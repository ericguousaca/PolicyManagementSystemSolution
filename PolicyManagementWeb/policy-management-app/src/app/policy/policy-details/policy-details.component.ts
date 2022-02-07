import { PolicyDetail } from './../../models/policy-detail.model';
import { Component, Input, OnInit } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { SortDescriptor, orderBy } from "@progress/kendo-data-query";

@Component({
  selector: 'app-policy-details',
  templateUrl: './policy-details.component.html',
  styleUrls: ['./policy-details.component.css'],
})
export class PolicyDetailsComponent implements OnInit {
  @Input() policyDetails?: PolicyDetail[];

  public gridViewData!: GridDataResult;
  public pageSize: number = 5;
  public skip = 0;

  public sortDesc: SortDescriptor[] = [
    {
      field: "id",
      dir: "asc",
    },
  ];

  constructor() {}

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    if (this.policyDetails) {
      this.policyDetails = orderBy(this.policyDetails, this.sortDesc);

      this.gridViewData = {
        data: this.policyDetails.slice(this.skip, this.skip + this.pageSize),
        total: this.policyDetails.length,
      };
    }
  }

  public pageChange(event: unknown): void {
    this.skip = (event as PageChangeEvent).skip;
    this.loadItems();
  }

  public sortChange(sort: unknown): void {
    this.sortDesc = sort as SortDescriptor[];
    this.loadItems();
  }
}
