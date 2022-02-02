using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.ViewModels;

namespace PolicyApiLibrary.Builders
{
    public interface IPolicyDetailBuilder
    {
        void BuildMainBodySection();
        void BuildMaturityAmountSection();
        void BuildPolicyTypeSection();
        void BuildUserTypeSection();
        PolicyDetailViewModel GetPolicyDetail();
        void SetupBuilder(Policy policy);
    }
}
