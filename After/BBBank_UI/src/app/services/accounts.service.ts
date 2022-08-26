import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/internal/Observable";
import { environment } from "src/environments/environment";
import { GetAccountByXResponse } from "../models/account-by-x";

@Injectable({
    providedIn: 'root',
  })
export default class AccountsService {
    constructor(private httpClient: HttpClient) { }
  
    getAccountByAccountNumber(accountNumber: string): Observable<GetAccountByXResponse> {
      return this.httpClient.get<GetAccountByXResponse>(`${environment.apiUrlBase}Accounts/GetAccountByAccountNumber/${accountNumber}`);
    }
  }