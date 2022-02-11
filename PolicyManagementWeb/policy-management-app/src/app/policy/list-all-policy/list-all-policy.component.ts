import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import {
  GridDataResult,
  DataStateChangeEvent,
} from '@progress/kendo-angular-grid';
import { PolicyDetail } from 'src/app/models/policy-detail.model';
import { PolicyDataService } from 'src/app/services/policy-data.service';
import { SortDescriptor, State } from '@progress/kendo-data-query';

@Component({
  selector: 'app-list-all-policy',
  templateUrl: './list-all-policy.component.html',
  styleUrls: ['./list-all-policy.component.css'],
})
export class ListAllPolicyComponent implements OnInit {
  policyDetails!: PolicyDetail[];
  error?: string;

  viewData!: GridDataResult;
  state: State = {
    skip: 0,
    take: 5,
    sort: [
      {
        field: 'id',
        dir: 'asc',
      },
    ],
  };

  public defaultSortDesc: SortDescriptor[] = [
    {
      field: 'id',
      dir: 'asc',
    },
  ];

  constructor(private policyDatatService: PolicyDataService) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.policyDatatService
      .getAllPolicy(
        this.state.skip,
        this.state.take,
        this.state.sort == undefined ? 'Id' : this.state.sort[0].field,
        this.state.sort == undefined ? 'asc' : this.state.sort[0].dir
      )
      .subscribe(
        (result) => {
          this.viewData = result;
        },
        (err) => {
          this.error = err;
          console.log(err);
        }
      );
  }

  public dataStateChange(newState: DataStateChangeEvent): void {
    this.state = newState;

    console.log(this.state);

    this.loadData();
  }
}
