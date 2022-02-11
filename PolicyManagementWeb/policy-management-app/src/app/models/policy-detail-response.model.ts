import { PolicyDetail } from "./policy-detail.model";

export class PolicyDetailResponse {
  policyDetails: PolicyDetail[] = [];
  totalCount:number = 0;
}
