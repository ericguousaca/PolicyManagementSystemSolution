export class PolicyDetail {
  id!: number;
  policyId!: string;
  policyName!: string;
  startDate!: Date;
  durationInYears!: number;
  companyName?: string;
  initialDeposit!: number;
  policyType!: string;
  userTypes!: string[];
  termsPerYear!: number;
  termAmount!: number;
  interest!: number;
  maturityAmount!: number;
}
