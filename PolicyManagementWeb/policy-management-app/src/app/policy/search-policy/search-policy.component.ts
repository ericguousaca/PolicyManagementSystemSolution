import { PolicyDetail } from 'src/app/models/policy-detail.model';
import { SearchPolicyParams } from './../../models/search-policy-params.model';
import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { PolicyDataService } from 'src/app/services/policy-data.service';
import { AppConstants } from 'src/app/models/app-constants.model';
import * as SignalR from '@microsoft/signalr';

@Component({
  selector: 'app-search-policy',
  templateUrl: './search-policy.component.html',
  styleUrls: ['./search-policy.component.css'],
})
export class SearchPolicyComponent implements OnInit {
  message?: string;
  error?: string;
  searchPolicyParams: SearchPolicyParams = new SearchPolicyParams();
  submittedParams?: SearchPolicyParams;
  policyDetails?: PolicyDetail[];
  searchPolicyForm!: FormGroup;

  private baseUrl: string = 'https://localhost:44321';
  private hubConnection!: SignalR.HubConnection;

  constructor(
    private formBuilder: FormBuilder,
    private policyDatatService: PolicyDataService
  ) {
    this.searchPolicyForm = this.formBuilder.group({
      policyType: [''],
      policyId: [''],
      policyName: [''],
      companyName: [''],
      numberOfYears: [1],
    });
  }

  ngOnInit(): void {}

  get policyType() {
    return this.searchPolicyForm.value['policyType'];
  }

  get policyId() {
    return this.searchPolicyForm.value['policyId'];
  }

  get policyName() {
    return this.searchPolicyForm.value['policyName'];
  }

  get companyName() {
    return this.searchPolicyForm.value['companyName'];
  }

  get numberOfYears() {
    return this.searchPolicyForm.value['numberOfYears'];
  }

  onDirectSearch() {
    this.message = undefined;
    this.error = undefined;
    this.submittedParams = undefined;
    this.policyDetails = undefined;

    this.assignFormValue();

    this.policyDatatService.searchPolicy(this.searchPolicyParams).subscribe(
      (data: PolicyDetail[]) => {
        //console.log(data);
        this.policyDetails = data;
        if (this.policyDetails?.length > 0) {
          this.message = 'Searched policy results succss.';
        } else {
          this.message = 'Could not find the policy by Search Conditions.';
        }
      },
      (err) => {
        this.error = err;
        console.log(err);
      }
    );

    //console.log(this.searchPolicyForm.value);
    //console.log(this.searchPolicyParams);
  }

  onSubmitSearchPolicyParams() {
    this.message = undefined;
    this.error = undefined;
    this.submittedParams = undefined;
    this.policyDetails = undefined;

    this.assignFormValue();

    this.hubConnection = new SignalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/PolicyHub`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Hub Connection started'))
      .then(() => {
        this.hubConnection.invoke('getconnectionid').then((connId) => {
          console.log('ConnectionId: ' + connId);
          this.searchPolicyParams.searchId = connId;

          this.policyDatatService
            .submitSearchPolicyParams(this.searchPolicyParams)
            .subscribe(
              (data: SearchPolicyParams) => {
                //console.log(data);
                this.submittedParams = data;
              },
              (err) => {
                this.error = err;
                //console.log(err);
              }
            );
        });
      })
      .catch((err) => console.log(`Hub Connection Error:${err}`));

    this.hubConnection.on('SearchPolicyAsync', (status, data) => {
      if (status == AppConstants.SEARCH_SENT) {
        this.message = 'Search command sent, please wait....';
        this.policyDetails = undefined;
        console.log(this.message);
      }
      if (status == AppConstants.RESULT_RECEIVED) {
        this.message =
          'Search results received and in processing, please wait...';
        this.policyDetails = undefined;
        console.log(this.message);
      }
      if (status == AppConstants.RESULT_AVAILABLE) {
        this.message = 'Search results available below:';
        this.policyDetails = data;
        // this.hubConnection.stop();
        console.log(this.message);
        console.log(this.policyDetails);
      }
      if (status == AppConstants.SEARCH_ERROR) {
        this.error = 'Error happened...';
        this.policyDetails = undefined;
        console.log(this.message);
      }
    });
  }

  getPolicySearchResults(searchId: string) {
    this.message = undefined;
    this.error = undefined;
    this.submittedParams = undefined;
    this.policyDetails = undefined;

    this.policyDatatService.getPolicySearchResults(searchId).subscribe(
      (data: PolicyDetail[]) => {
        //console.log(data);
        this.policyDetails = data;
        if (this.policyDetails?.length > 0) {
          this.message = 'Got search policy results succss.';
        } else {
          this.message = 'Could not find the policy by Search Conditions.';
        }
      },
      (err) => {
        this.error = err;
        //console.log(err);
      }
    );
  }

  private assignFormValue() {
    this.searchPolicyParams.policyType = this.policyType;
    this.searchPolicyParams.policyId = this.policyId;
    this.searchPolicyParams.policyName = this.policyName;
    this.searchPolicyParams.companyName = this.companyName;
    this.searchPolicyParams.numberOfYears =
      this.numberOfYears == null ? 0 : this.numberOfYears;
  }
}
