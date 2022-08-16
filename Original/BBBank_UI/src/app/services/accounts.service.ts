import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

import { GetAccountByXResponse } from '../models/account-by-x';


@Injectable({
  providedIn: 'root',
})
export default class AccountsService {
  constructor(private httpClient: HttpClient) { }

  getAccountByAccountNumber(accountNumber: string): Observable<GetAccountByXResponse> {
    return this.httpClient.get<GetAccountByXResponse>(`${environment.apiUrlBase}Accounts/GetAccountByAccountNumber/${accountNumber}`);
  }

}