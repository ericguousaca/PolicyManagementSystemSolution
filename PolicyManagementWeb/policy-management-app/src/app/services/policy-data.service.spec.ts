import { MOCKGRIDDATARESULT, POLICYDETAILS } from './../../../test-data/policy-data';
import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { PolicyDataService } from './policy-data.service';
import { AppConstants } from '../models/app-constants.model';
import { SearchPolicyParams } from '../models/search-policy-params.model';

describe('PolicyDataService', () => {
  let baseUrl: string = AppConstants.Base_Url;

  let getAllPolicyUrl = baseUrl + '/api/v1.0/Policy/GetAll';
  let directSearchPolicyUrl = baseUrl + '/api/v1.0/Policy/Searches';

  let submitSearchPolicyUrl =
    baseUrl + '/api/v1.0/PolicySearch/SubmitSearchPolicyCommand';
  let getPolicySearchResultsUrl =
    baseUrl + '/api/v1.0/PolicySearch/GetSearchPolicyResults';

  let mockGridDataResult = MOCKGRIDDATARESULT;

  let service: PolicyDataService;
  let testingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PolicyDataService],
    });

    service = TestBed.inject(PolicyDataService);
    testingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    testingController.verify();
  });

  it('should create the service object', () => {
    expect(service).toBeTruthy('Policy Data Service object should be created');
  });

  it('should get all policies', () => {
    service.getAllPolicy().subscribe((resultData) => {
      expect(resultData).toBeTruthy('No policy details returned');
      expect(resultData.data.length).toBe(5, 'Total policy detail items in View are not 5');
      expect(resultData.total).toBe(18, 'The mocked total policy details are not 18');
    });

    const req = testingController.expectOne(
      (req) => req.url == getAllPolicyUrl
    );
    expect(req.request.method).toEqual('GET');
    req.flush(mockGridDataResult);
  });

  it('should retrun correct search results', () => {
    let searchParams: SearchPolicyParams = new SearchPolicyParams();
    searchParams.policyId = 'CP-2021-001';
    searchParams.policyType = 'Child Plans';
    searchParams.policyName = 'Test Policy #1';
    searchParams.companyName = 'Test Company 1';
    searchParams.numberOfYears = 3;

    service.searchPolicy(searchParams).subscribe(function (data) {
      expect(data).toBeTruthy('No policy details returned');
      expect(data.length).toBe(1, 'Search result shall be only one item');
    });

    const req = testingController.expectOne(
      (req) => req.url == directSearchPolicyUrl
    );
    expect(req.request.method).toEqual('POST');
    req.flush(
      mockGridDataResult.data.filter(
        (x) =>
          x.policyId == searchParams.policyId &&
          x.policyType.includes(searchParams.policyType) &&
          x.policyName.includes(searchParams.policyName) &&
          x.companyName?.includes(searchParams.companyName) &&
          x.durationInYears == searchParams.numberOfYears
      )
    );
  });
});
