import { ApiResponse } from "./api-Response";

 export class AccountByX {
    accountId: string;
    accountTitle: string;
    userImageUrl: string;
    currentBalance: number;
    accountStatus: string;
    accountNumber: string;
  }

  export interface GetAccountByXResponse extends ApiResponse {
    result: AccountByX
  }