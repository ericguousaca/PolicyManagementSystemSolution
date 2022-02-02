import { PolicyDetail } from './../../models/policy-detail.model';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-policy-details',
  templateUrl: './policy-details.component.html',
  styleUrls: ['./policy-details.component.css']
})
export class PolicyDetailsComponent implements OnInit {
  @Input() policyDetails?:PolicyDetail[];

  constructor() { }

  ngOnInit(): void {
  }

}
