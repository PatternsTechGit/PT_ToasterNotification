import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {  LineGraphDataResponse } from '../models/line-graph-data';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export  class TransactionService {
  constructor(private httpclient: HttpClient) {}
  getLast12MonthBalances(accountId?: string): Observable<LineGraphDataResponse> {
    if (accountId === null) {
      return this.httpclient.get<LineGraphDataResponse>(`${environment.apiUrlBase}Transaction/GetLast12MonthBalances`);
    }
    return this.httpclient.get<LineGraphDataResponse>(`${environment.apiUrlBase}Transaction/GetLast12MonthBalances/${accountId}`);
  }
}
