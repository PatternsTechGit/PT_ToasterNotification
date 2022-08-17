import { Component, OnInit } from '@angular/core';
import { LineGraphData } from './models/line-graph-data'
import { AccountByX } from './models/account-by-x';
import DepositRequest from './models/deposit-request';
import AccountsService from './services/accounts.service';
import { TransactionService } from './services/transaction.service';
import { NotificationService } from './notification.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {

  title = 'BBBankUI';
  lineGraphData: LineGraphData;
  account: AccountByX;
  message: string;
  amount: number;
  constructor(private accountsService: AccountsService,private transactionService:TransactionService,
    private notifyService: NotificationService) { }

  ngOnInit(): void {
    this.initialize();
    this.account.accountNumber = '0001-1001';
    
    this.getToAccount();
  }

  getToAccount() {
    this.accountsService
      .getAccountByAccountNumber(this.account.accountNumber)
      .subscribe({
        next: (data) => {
          if (data.statusCode == 204) {
            this.initialize();
            this.message = String(data.result);
          }
          else {
            this.account = data.result
          }
        },
        error: (error) => {
          this.message = String(error);
        },
      });
  }

  cancel(){
    this.amount=0;
  }
    
  deposit() {
    const depositRequest: DepositRequest = {
      accountId: this.account.accountNumber,
      amount: this.amount
    };
    this.transactionService
      .deposit(depositRequest)
      .subscribe({
        next: (data) => {
          if (data.statusCode == 204) {
            this.notifyService.showInfo(data.result);
            this.message = String(data.result);
            this.initialize();

          } else {
            this.notifyService.showSuccess(data.message);
            this.amount=0;
          }
        },
        error: (err) => {
                    this.notifyService.showError(err.error);
        },
      });
  } 

  initialize() {
    this.message = '';
    this.account = new AccountByX();
    this.account.userImageUrl = '../../../assets/images/No-Image.png'
  }
}
