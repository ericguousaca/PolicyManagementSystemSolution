import { Component, OnInit } from '@angular/core';
import { PolicyDetail } from 'src/app/models/policy-detail.model';
import { PolicyDataService } from 'src/app/services/policy-data.service';

@Component({
  selector: 'app-list-all-policy',
  templateUrl: './list-all-policy.component.html',
  styleUrls: ['./list-all-policy.component.css'],
})
export class ListAllPolicyComponent implements OnInit {
  policyDetails!: PolicyDetail[];
  error?:string;
  constructor(private policyDatatService: PolicyDataService) {}

  ngOnInit(): void {
    this.policyDatatService.getAllPolicy().subscribe((data: PolicyDetail[]) => {
      // console.log(JSON.stringify(data));
      this.policyDetails = data;
    },
    err =>{
      this.error = err;
      console.log(err);
    }
    );
  }
}
