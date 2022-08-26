import { ApiResponse } from "./api-Response";

export default class DepositRequest {
  accountId: string;
  amount: number;
}

export interface DepositResponse extends ApiResponse {
  result: string
}