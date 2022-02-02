import { PageNoFoundComponent } from './page-no-found/page-no-found.component';
import { SearchPolicyComponent } from './policy/search-policy/search-policy.component';
import { ListAllPolicyComponent } from './policy/list-all-policy/list-all-policy.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListAllCustomerComponent } from './customer/list-all-customer/list-all-customer.component';

const routes: Routes = [
  {
    path: 'list-all-policy',
    component: ListAllPolicyComponent,
    //resolve: { employeeList: EmployeeListResolverService },
  },
  {
    path: 'search-policy',
    component: SearchPolicyComponent,
    //canDeactivate: [CreateEmployeeCanDeactivateGuardService],
  },
  {
    path: 'list-all-customer',
    component: ListAllCustomerComponent,
    //canActivate: [EmployeeDetailsGuardService],
  },
  { path: '', redirectTo: '/list-all-policy', pathMatch: 'full' },
  { path: 'notfound', component: PageNoFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
