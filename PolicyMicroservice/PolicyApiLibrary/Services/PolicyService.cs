using PolicyApiLibrary.Builders;
using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.Models;
using PolicyApiLibrary.Repositories;
using PolicyApiLibrary.Services.ServiceResponse;
using PolicyApiLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PolicyApiLibrary.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepo;
        private readonly IUserTypeRepository _userTypeRepo;
        private readonly IPolicyTypeRepository _policyTypeRepo;
        private readonly IDirector _policyDetailDirector;
        private readonly IPolicyDetailBuilder _policyDetailBuilder;

        public PolicyService(IPolicyRepository policyRepo, IUserTypeRepository userTypeRepo, IPolicyTypeRepository policyTypeRepo,
            IDirector policyDetailDirector, IPolicyDetailBuilder policyDetailBuilder)
        {
            this._policyRepo = policyRepo;
            this._userTypeRepo = userTypeRepo;
            this._policyTypeRepo = policyTypeRepo;
            this._policyDetailDirector = policyDetailDirector;
            this._policyDetailBuilder = policyDetailBuilder;
        }

        public string GeneratePolicyRegitserHtmlMessage(Policy policy, string templateFilePath)
        {
            string htmlMessage = "";

            if (File.Exists(templateFilePath))
            {
                htmlMessage = File.ReadAllText(templateFilePath);

                htmlMessage = htmlMessage.Replace("$policy_id$", policy.PolicyId);
                htmlMessage = htmlMessage.Replace("$policy_start_date$", policy.StartDate.ToString("dd'/'MM'/'yyyy"));
                htmlMessage = htmlMessage.Replace("$policy_end_date$", policy.StartDate.AddYears(policy.DurationInYear).ToString("dd'/'MM'/'yyyy"));
                htmlMessage = htmlMessage.Replace("$policy_seq_num$", policy.PolicyId.Split("-")[2]);
                htmlMessage = htmlMessage.Replace("$policy_type$", policy.PolicyType.TypeName);
                htmlMessage = htmlMessage.Replace("$policy_register_link$", @"https://localhost:44313/api/v1.0/Policy/Register");
            }
            else
            {
                htmlMessage = "Success register policy, but could not find the Html Message template, please contact IT support.";
            }

            return htmlMessage;
        }

        public async Task<GetAllPolicyDetailsResponse> GetAllPolicyDetailsAsync()
        {
            GetAllPolicyDetailsResponse response = new GetAllPolicyDetailsResponse();

            try
            {
                List<PolicyDetailViewModel> vmPolicyDetailList = new List<PolicyDetailViewModel>();

                IEnumerable<Policy> policies = await this._policyRepo.GetAllPoliciesAsync();

                foreach (Policy policy in policies)
                {
                    PolicyDetailViewModel vmPolicyDetail = this.buildPolicyDetailViewMode(policy);

                    vmPolicyDetailList.Add(vmPolicyDetail);
                }

                response.Success = true;
                response.PolicyDetails = vmPolicyDetailList;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Exception = ex;
                response.InternalMessge = ex.Message;
                response.PublicMessage = "An exception happend to get all policy details.";

                return response;
            }
        }

        public async Task<RegisterPolicyResponse> RegisterPolicyAsync(PolicyViewModel vmPolicy)
        {
            RegisterPolicyResponse response = new RegisterPolicyResponse();

            response.Success = false;

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Step #1: Check Policy name exitsing (Policy name must be unqiue in Policy Table)
                    bool isPolicyNameExisting = this._policyRepo.IsPolicyNameExisting(vmPolicy.PolicyName);

                    if (isPolicyNameExisting == true)
                    {
                        response.Success = false;
                        response.InternalMessge = $"Policy name \"{vmPolicy.PolicyName}\" is existing in system, please change to another name.";
                        response.PublicMessage = response.InternalMessge;

                        return response;
                    }

                    // Step #2: Get selected User Type List 
                    List<UserType> selectedUserTypeList = new List<UserType>();
                    List<UserType> userTypeList = this._userTypeRepo.GetUserTypes().ToList();
                    foreach (string userTypeName in vmPolicy.UserTypes)
                    {
                        UserType userType = userTypeList.Where(x => x.TypeName.ToUpper() == userTypeName.Trim().ToUpper()).FirstOrDefault();

                        if (userType == null)
                        {
                            response.Success = false;
                            response.InternalMessge = $"Could not get User Type from {userTypeName}.";
                            response.PublicMessage = response.InternalMessge;

                            return response;
                        }


                        if (selectedUserTypeList.Where(x => x.Id == userType.Id).FirstOrDefault() == null)
                        {
                            selectedUserTypeList.Add(userType);
                        }
                    }

                    // Step #3: Generate new Policy Id
                    DateTime startDate = DateTime.MinValue;

                    if (!DateTime.TryParseExact(vmPolicy.StartDate, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out startDate))
                    {
                        response.Success = false;
                        response.InternalMessge = $"Could not parse the StartDate {vmPolicy.StartDate} to be Datetime format.";
                        response.PublicMessage = response.InternalMessge;

                        return response;
                    }

                    PolicyType selectedPolicyType = this._policyTypeRepo.GetPolicyType(vmPolicy.PolicyType);

                    if (selectedPolicyType == null)
                    {
                        response.Success = false;
                        response.InternalMessge = $"Could not find the selected Policy Type {vmPolicy.PolicyType} from database.";
                        response.PublicMessage = response.InternalMessge;

                        return response;
                    }


                    Policy lastPolicyInType = await this._policyRepo.GetLastPolicyByPolicyTypeIdAsync(selectedPolicyType.Id);
                    int seqNum = 1;

                    if (lastPolicyInType != null)
                    {
                        string[] items = lastPolicyInType.PolicyId.Split("-");

                        if (items.Length != 3)
                        {
                            response.Success = false;
                            response.InternalMessge = $"The last policy ID {lastPolicyInType.PolicyTypeId} has wrong format.";
                            response.PublicMessage = response.InternalMessge;

                            return response;
                        }

                        string lastSeqNumStr = items[2];

                        int lastSeqNum = 0;

                        if (!int.TryParse(lastSeqNumStr, out lastSeqNum))
                        {
                            response.Success = false;
                            response.InternalMessge = $"The last policy ID {lastPolicyInType.PolicyTypeId}'s sequence number part has wrong format.";
                            response.PublicMessage = response.InternalMessge;

                            return response;
                        }

                        seqNum = lastSeqNum + 1;
                    }

                    string seqNumStr = "";
                    seqNumStr = seqNum <= 999 ? seqNum.ToString("D3") : seqNum.ToString();

                    string newPolicyId = selectedPolicyType.Id + "-" + startDate.Year.ToString() + "-" + seqNumStr;


                    // Step #4: Added policy to DB
                    Policy newPolicy = new Policy();
                    newPolicy.PolicyId = newPolicyId;
                    newPolicy.PolicyName = vmPolicy.PolicyName.Trim();
                    newPolicy.StartDate = startDate;
                    newPolicy.DurationInYear = vmPolicy.DurationInYears;
                    newPolicy.ComapnyName = vmPolicy.CompanyName;
                    newPolicy.InitialDeposit = vmPolicy.InitialDeposit;
                    newPolicy.PolicyTypeId = selectedPolicyType.Id;
                    newPolicy.TermsPerYear = vmPolicy.TermsPerYear;
                    newPolicy.TermAmount = vmPolicy.TermAmount;
                    newPolicy.Interest = vmPolicy.Interest;

                    newPolicy = await this._policyRepo.AddPolicyAsync(newPolicy, selectedUserTypeList);

                    response.Success = true;
                    response.Policy = newPolicy;
                    response.VmPolicyDetail = this.buildPolicyDetailViewMode(newPolicy);

                    scope.Complete();
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Exception = ex;
                response.InternalMessge = ex.Message;
                response.PublicMessage = "System exception occured. please try again, if contiunue see the error, please contact IT department.";

                return response;
            }
        }

        public async Task<SearchPolicyDetailsResponse> SearchPolicyDetailsAsync(SearchPolicyParamModel paramModel)
        {
            SearchPolicyDetailsResponse response = new SearchPolicyDetailsResponse();
            response.SearchParamModel = paramModel;

            try
            {
                List<PolicyDetailViewModel> vmPolicyDetailList = new List<PolicyDetailViewModel>();

                IEnumerable<Policy> policies = await this._policyRepo.SearchPolicy(paramModel);

                foreach (Policy policy in policies)
                {
                    PolicyDetailViewModel vmPolicyDetail = this.buildPolicyDetailViewMode(policy);

                    vmPolicyDetailList.Add(vmPolicyDetail);
                }

                response.Success = true;
                response.PolicyDetails = vmPolicyDetailList;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Exception = ex;
                response.InternalMessge = ex.Message;
                response.PublicMessage = "An exception happend to get all policy details.";

                return response;
            }
        }

        private PolicyDetailViewModel buildPolicyDetailViewMode(Policy policy)
        {
            this._policyDetailDirector.Construct(this._policyDetailBuilder, policy);
            PolicyDetailViewModel vmPolicy = this._policyDetailBuilder.GetPolicyDetail();

            return vmPolicy;
        }
    }
}
