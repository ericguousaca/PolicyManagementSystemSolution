import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ListAllPolicyComponent } from './policy/list-all-policy/list-all-policy.component';
import { PolicyDetailsComponent } from './policy/policy-details/policy-details.component';
import { SearchPolicyComponent } from './policy/search-policy/search-policy.component';
import { ListAllCustomerComponent } from './customer/list-all-customer/list-all-customer.component';
import { PageNoFoundComponent } from './page-no-found/page-no-found.component';

@NgModule({
  declarations: [
    AppComponent,
    ListAllPolicyComponent,
    PolicyDetailsComponent,
    SearchPolicyComponent,
    ListAllCustomerComponent,
    PageNoFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
