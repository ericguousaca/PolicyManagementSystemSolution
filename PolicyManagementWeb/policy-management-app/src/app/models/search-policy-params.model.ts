export class SearchPolicyParams {
  searchId: string = '';
  policyType!: string;
  policyId!: string;
  policyName!: string;
  companyName!: string;
  numberOfYears!: number;
  searchTime?:Date;
}
