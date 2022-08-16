import { Component, OnInit } from '@angular/core';
import { LineGraphData } from './models/line-graph-data'
import { TransactionService } from './services/transaction.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {

  title = 'BBBankUI';
  lineGraphData: LineGraphData;

  constructor(private transactionService: TransactionService) { }

  ngOnInit(): void {
    this.transactionService
      .getLast12MonthBalances('')
      .subscribe({
        next: (data) => {
          this.lineGraphData = data.result;
        },
        error: (error) => {
          console.log(error.responseException.exceptionMessage);
        },
      });
  }
}
