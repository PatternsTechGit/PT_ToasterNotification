import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LineGraphDataResponse } from '../models/line-graph-data';
import { environment } from '../../environments/environment';
import DepositRequest, { DepositResponse } from '../models/deposit-request';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  constructor(private httpclient: HttpClient) { }
  getLast12MonthBalances(accountId?: string): Observable<LineGraphDataResponse> {
    if (accountId === null) {
      return this.httpclient.get<LineGraphDataResponse>(`${environment.apiUrlBase}Transaction/GetLast12MonthBalances`);
    }
    return this.httpclient.get<LineGraphDataResponse>(`${environment.apiUrlBase}Transaction/GetLast12MonthBalances/${accountId}`);
  }

  deposit(depositRequest: DepositRequest): Observable<DepositResponse> {
    const headers = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }
    return this.httpclient.post<DepositResponse>(`${environment.apiUrlBase}Transaction/Deposit`, JSON.stringify(depositRequest), headers);
  }


}
