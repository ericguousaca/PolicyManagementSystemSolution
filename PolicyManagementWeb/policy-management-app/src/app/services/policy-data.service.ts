import { PolicyDetail } from 'src/app/models/policy-detail.model';
import { SearchPolicyParams } from './../models/search-policy-params.model';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AppConstants } from '../models/app-constants.model';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { PolicyDetailResponse } from '../models/policy-detail-response.model';

@Injectable({
  providedIn: 'root',
})
export class PolicyDataService {
  // Make sure below base Url working in Common Api Gateway, like Ocelot etc.
  baseUrl: string = AppConstants.Base_Url; //'https://localhost:44330';

  getAllPolicyUrl = this.baseUrl + '/api/v1.0/Policy/GetAll';
  directSearchPolicyUrl = this.baseUrl + '/api/v1.0/Policy/Searches';

  submitSearchPolicyUrl =
    this.baseUrl + '/api/v1.0/PolicySearch/SubmitSearchPolicyCommand';
  getPolicySearchResultsUrl =
    this.baseUrl + '/api/v1.0/PolicySearch/GetSearchPolicyResults';

  constructor(private httpClient: HttpClient) {}

  getAllPolicy(
    skip: number = -1,
    pageSize: number = 0,
    sortBy: string = 'Id',
    sortDirection: string = 'ASC'
  ): Observable<GridDataResult> {
    console.log(this.getAllPolicyUrl);
    return this.httpClient
      .get<PolicyDetailResponse>(this.getAllPolicyUrl, {
        params: {
          skip: skip,
          pageSize: pageSize,
          sortBy: sortBy,
          sortDirection: sortDirection,
        },
      })
      .pipe(
        map((res: PolicyDetailResponse) => {
          const result = <GridDataResult>{
            data: res.policyDetails,
            total: res.totalCount,
          };
          return result;
        }),
        catchError(this.handleError)
      );
  }

  searchPolicy(searchParams: SearchPolicyParams): Observable<PolicyDetail[]> {
    return this.httpClient
      .post<PolicyDetail[]>(this.directSearchPolicyUrl, searchParams, {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
      })
      .pipe(catchError(this.handleError));
  }

  submitSearchPolicyParams(
    searchParams: SearchPolicyParams
  ): Observable<SearchPolicyParams> {
    return this.httpClient
      .post<SearchPolicyParams>(this.submitSearchPolicyUrl, searchParams, {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
      })
      .pipe(catchError(this.handleError));
  }

  getPolicySearchResults(searchId: string): Observable<PolicyDetail[]> {
    return this.httpClient
      .get<PolicyDetail[]>(`${this.getPolicySearchResultsUrl}/${searchId}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(errorResponse: HttpErrorResponse) {
    if (errorResponse.error instanceof ErrorEvent) {
      console.error('Client Side Error: ', errorResponse.error.message);
    } else {
      console.error('Server Side Error: ', errorResponse);
    }

    return throwError(
      'There is a problem with the service. We are notified & working on it. Please try again later.'
    );
  }
}
