# Toaster Notification with Handling No Content

## What is HTTP 204 - No Content
The HTTP 204 No Content status response code indicates that a request has succeeded, but the server side respond with **no body** or returned **null** result.

## What is Toaster Notification

The Angular Toast is a small, non-blocking notification pop-up. A toast is shown to users with readable message content at the top of the screen or at a specific target and disappears automatically after a few seconds (time-out) with different animation effects. The control has various built-in options for customizing visual elements, animations, durations, and dismissing toasts.



# About this exercise

## Backend Code Base:

Previously we have developed an **API** solution in asp.net core in which we have

* EF Code first approach to generate database of a fictitious bank application called **BBBank**.
* We have implemented **AutoWrapper** in BBankAPI project. 

For more details see [data seeding](https://github.com/PatternsTechGit/PT_AzureSql_EFDataSeeding) lab.

## Frontend Codebase
Previously we have angular application in which we have

* FontAwesome library for icons.
* Bootstrap library for styling.
* Created client side models to receive data.
* Created transaction service to call the API.
* Fixed the CORS error on the server side.
* Populated html table, using data returned by API.
* Handled AutoWrapper results.

For more details see [Angular calling API](https://github.com/PatternsTechGit/PT_AngularCallingAPI) lab.


## **In this exercise**

In this exercise again we will be working on both **frontend** & **backend** codebase.

**Backend Codebase**

#### On server side we will:
* Create an **account controller** with method `GetAccountByAccountNumber`.
* Create an **account service** and a contract for this service in the **Service** project.
* Create HttpPost `Deposit` method in **transaction controller**.
* Implement deposit fund by account number, If account number does not matches then will return **no content**.


**Frontend Codebase**
#### On frontend side we will:
* Create a new form with Image & input field.
* Load the account information on load.
* Create client side **models** to map data for API.
* Create the **account service** to call the **GetAccountByAccountNumber** method of API.
* Implement **Deposit** method in **transaction service** to call the API.
* Implement Angular Toaster Notifications.


# Server Side Implementation

Follow the below steps to implement server side code changes:

## Step 1: Create AccountByUserResponse class

We will create a new class named **AccountByUserResponse** in **Entities** project under **Responses** folder which will contain the account related information and user Image url as below :

```cs
public class AccountByUserResponse
    {
        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountTitle { get; set; }
        public decimal CurrentBalance { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountStatus AccountStatus { get; set; }
        public string UserImageUrl { get; set; }
    }
```

## Step 2: Creating Interface for Account Service

In **Services** project create an interface (contract) in **Contracts** folder to implement the separation of concerns.
It will make our code testable and injectable as a dependency.

```csharp
public interface IAccountsService
{
    Task<AccountByUserResponse> GetAccountByAccountNumber(string accountNumber);
}
```

## Step 3: Implementing Account Service 

In **Services** project we will be implementing account service. Create new file **AccountService.cs** In this file we will be implementing **IAccountsService** interface.

 In `GetAccountByAccountNumber` method we are checking if account exists by accountNumber or does not exists. If the account exist then return the AccountByUserResponse object otherwise return null as below   

```csharp
 public class AccountService : IAccountsService
    {
        private readonly BBBankContext _bbBankContext;
        public AccountService(BBBankContext BBBankContext)
        {
            _bbBankContext = BBBankContext;
        }
        public async Task<AccountByUserResponse> GetAccountByAccountNumber(string accountNumber)
        {
            var account =  _bbBankContext.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null)
                return null;
            else
            {
                return new AccountByUserResponse
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountStatus = account.AccountStatus,
                    AccountTitle = account.AccountTitle,
                    CurrentBalance = account.CurrentBalance,
                    UserImageUrl = account.User.ProfilePicUrl
                };
            }
        }
    }
```

## Step 4: Dependency Injecting BBBankContext & AccountService 

In `Program.cs` file we will inject the **IAccountsService** to services container, so that we can use the relevant object in services.

```csharp
builder.Services.AddScoped<IAccountsService, AccountService>();
```

## Step 5: SettingUp Accounts Controller 

Create a new API controller named `AccountsController` and inject the `IAccountsService` using the constructor.

```csharp
private readonly IAccountsService _accountsService;
public AccountsController(IAccountsService accountsService)
{
    _accountsService = accountsService;
}
```

Now we will create an API method **GetAccountByAccountNumber** in `AccountsController` to call the service to check either account exists or not.

```csharp
[Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet]
        [Route("GetAccountByAccountNumber/{accountNumber}")]
        public async Task<ApiResponse> GetAccountByAccountNumber(string accountNumber)
        {
            var account = await _accountsService.GetAccountByAccountNumber(accountNumber);
            if (account == null)
                return new ApiResponse($"no Account exists with accountnumber {accountNumber}", 204);
            return new ApiResponse("Account By Number Returned", account);
        }
    }
```
If the account exists then we will return **ApiResponse** with result.

IF the account does not exists or null then we will return **ApiResponse** with message and **204 status code**. 

## Step 6: Creating DepositFunds method 

Go to **ITransactionService** In Services project and create an a new method named `DepositFunds`.

```csharp
 public interface ITransactionService
    {
        Task<int> DepositFunds(DepositRequest depositRequest);
    }
```

## Step 7: Implementing Transaction Service 

In **Services** project we will be implementing account service. Open the **TransactionService** file and implement the DepositFunds method.

 In `DepositFunds` method we are checking if account exists by accountNumber or does not exists. If the account exist then we will be adding a transaction for relevant account otherwise return -1.   

```csharp
 public async Task<int> DepositFunds(DepositRequest depositRequest)
        {
            var account = _bbBankContext.Accounts.Where(x => x.AccountNumber == depositRequest.AccountId).FirstOrDefault();
            if (account == null)
                return -1;
            else
            {
                var transaction = new Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    TransactionAmount = depositRequest.Amount,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = TransactionType.Deposit
                };
                if(account.Transactions!=null)
                account.Transactions.Add(transaction);
                return 1;
            }
        }
```

## Step 8: SettingUp Transaction Controller 


Now we will create an API method **Deposit** in `TransactionController` to call the service to deposit the funds.

```csharp
 [HttpPost]
        [Route("Deposit")]
        public async Task<ApiResponse> Deposit(DepositRequest depositRequest)
        {
            var result = await _transactionService.DepositFunds(depositRequest);
            if (result == -1)
                return new ApiResponse($"no Account exists with accountId {depositRequest.AccountId}", 204);
            return new ApiResponse($"{depositRequest.Amount}$ Deposited");

        }
```

Thats It! on the server side, Run the project and see its working. 

# Frontend Implementation
Follow the below steps to implement frontend code changes:

## Step 1 : Configure Toaster Service

### **Install ngx-toastr library**

To install the ngx-toastr library run the npm command as below :

```
npm i ngx-toastr@13.0.0 
```
As our project is using  angular common v.13 so we will install the dependent toastr library version (13.0.0) otherwise for latest angular we can use command as below :

```
 npm i --save ngx-toastr
```

### **SettingUp toastr Style** 

After installing the library we will add toastr **css** style reference in `angular.json` file as below :

```json
"styles": [
              "./node_modules/bootstrap/dist/css/bootstrap.min.css",
			  "node_modules/@fortawesome/fontawesome-free/css/all.min.css",
              "src/styles.css",
			  "node_modules/ngx-toastr/toastr.css" 
            ]
```

### **Import Toastr Module**
Go to `app.module.ts` file and add **FormsModule,BrowserAnimationsModule,
    ToastrModule.forRoot()** reference in imports as below : 

```ts
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule ,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
  ]
```

## Step 2: Implement Notification Service
Create a new file `notification.service.ts` in **app** folder which will contains the common functions for showing the notification.

The common functions are :

* showSuccess -Will show the success notification message in **Green** Color. 
* showError - Will show the error notification message in **Red** Color.
* showInfo  -Will show the information notification message in **Blue** Color.
* showWarning -Will show the success notification in message **Orange** Color.


We will Inject the ToastrService and use its methods accordingly as below :


```ts
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
   
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
   
  constructor(private toastr: ToastrService) { }
   title :'BBBank';
  showSuccess(message:any){
      this.toastr.success(message, this.title)
  }
   
  showError(message:any){
      this.toastr.error(message, this.title)
  }
   
  showInfo(message:any){
      this.toastr.info(message, this.title)
  }
   
  showWarning(message:any){
      this.toastr.warning(message, this.title)
 }

}
```

## Step 3: Create/Update model classes

Go to `api-Response.ts` in **models** folder and add a new property **statusCode** which will contain the status code received from server side as below:

```ts
export interface ApiResponse {
    isError: boolean;
    message: string;
    statusCode: number;
    responseException: ResponseException;
}
```
Create a new file `account-by-x.ts`  in **models** folder and which will contain account related properties as below:
```ts
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
```

Create a new file `deposit-request.ts`  in **models** folder and which will contain deposit funds related properties as below:
```ts
import { ApiResponse } from "./api-Response";

export default class DepositRequest {
  accountId: string;
  amount: number;
}

export interface DepositResponse extends ApiResponse {
  result: string
}
```

## Step 4: Setup Account Service
Create a new file `accounts.service.ts` in **services** folder which will contain the `getAccountByAccountNumber` method which will call the API method to get account information by accountNumber as below :

```ts
export default class AccountsService {
  constructor(private httpClient: HttpClient) { }

  getAccountByAccountNumber(accountNumber: string): Observable<GetAccountByXResponse> {
    return this.httpClient.get<GetAccountByXResponse>(`${environment.apiUrlBase}Accounts/GetAccountByAccountNumber/${accountNumber}`);
  }
}
```

## Step 5:  Setup Transaction Service 
Go to `transaction.service.ts` in **services** folder and create a new method named `deposit` which will call the API method to deposit funds by accountNumber as below :

```ts
deposit(depositRequest: DepositRequest): Observable<DepositResponse> {
    const headers = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }
    return this.httpclient.post<DepositResponse>(`${environment.apiUrlBase}Transaction/Deposit`, JSON.stringify(depositRequest), headers);
  }
```


## Step 6: SettingUp UI 
Go to `app.component.html` and add Image control that is bind to the users profile picture, add labels and input field for deposit amount value. All these filed are bind as two way binding to `AccountByX` model.

Furthermore we will create 2 buttons `Deposit` and `Cancel`. On deposit button click we will call the transactionService to deposit the funds and on cancel button click we will clear the amount.

```html
<div class="container-fluid">
    <app-toolbar></app-toolbar>
</div>

<div class="row">
    <div class="col-12 col-sm-6">
        <div class="card card-user">
            <div class="card-body">
                <div class="author">
                    <div class="block block-one"></div>
                    <div class="block block-two"></div>
                    <div class="block block-three"></div>
                    <div class="block block-four"></div>

                    <img alt="..." class=" avatar" src="{{account?.userImageUrl}}" />
                    <h5 class="title">{{account?.accountTitle}}</h5>
                    <p class="account-number">{{account?.accountNumber}}</p>
                    <p class="balance">Balance: <b>$ {{account.currentBalance}}</b> <span
                            *ngIf="account?.accountStatus === 'Active'"
                            class="account-status active">Active</span><span
                            *ngIf="account?.accountStatus === 'InActive'"
                            class="account-status inactive">Inactive</span></p>
                    <div class="amount-cont">
                        <div class="error-message">
                            <p class="text-warning">
                                 {{message}}
                            </p>
                        </div>
                        <div class="form-row">
                            <div class="col-sm-2"></div>
                            <div class="col-sm-8 my-1">
                                <div class="input-group mb-2">
                                    <div class="input-group-prepend" style="margin-left: 50%">
                                        <div class="input-group-text">$</div>
                                    </div>
                                    <input type="number" [(ngModel)]="amount" class="form-control align-self-center" id="depositAmount" placeholder="Deposit Amount">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-6 mb-3">
        <button class="btn btn-light btn-block" (click)="cancel()" style="width: 30%;margin-left: 20%;">Cancel</button>
    </div>
    <div class="col-sm-6 mb-3">
        <button class="btn btn-danger btn-block" (click)="deposit()" style="width: 30%;margin-left: -50%;">Deposit</button>
    </div>
</div>

<router-outlet></router-outlet>
```


## Step 7: Getting Existing Account & Deposit Funds

Go to `app.component.ts` and create `getToAccount` function which will be called on **ngOnInit** life cycle. This method will call the 
 `getAccountByAccountNumber` method of `AccountsService`.
 Once the response is received from API then it will check the **statusCode**. If the statusCode is **204** then we will get the error/warning message from **data.result** object. Otherwise we will set the 
toAccount object with received result.

We will create `initializeTo` to empty the form, so that we can reset the form on **ngOnInit** and after **204 error** received.

We will create `deposit` method which will cal the **deposit** method of `TransactionService` to deposit the funds.

We will inject the `NotificationService` for **Toastr Notification** and call the notification service methods on response received from deposit method.

Here is the code as below :

```ts
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

```

## Step 8: SettingUp Styling
Go to `app.component.css` and add the following **css** for styling. 

```css

.card {
  position: relative;
  display: flex;
  flex-direction: column;
  min-width: 0;
  word-wrap: break-word;
  background-color: #ffffff;
  background-clip: border-box;
  border: 0.0625rem solid rgba(34, 42, 66, 0.05);
  border-radius: 0.2857rem;
}
.card {
  background: #27293d;
  border: 0;
  position: relative;
  width: 100%;
  margin-bottom: 30px;
  box-shadow: 0 1px 20px 0px rgba(0, 0, 0, 0.1);
}
.card .card-body {
  padding: 15px;
}
.card .card-body .card-description {
  color: rgba(255, 255, 255, 0.6);
}
.card .avatar {
  width: 30px;
  height: 30px;
  overflow: hidden;
  border-radius: 50%;
  margin-bottom: 15px;
}
.card-user {
  overflow: hidden;
}
.card-user .author {
  text-align: center;
  text-transform: none;
  margin-top: 25px;
}
.card-user .author a+p.description {
  margin-top: -7px;
}
.card-user .author .block {
  position: absolute;
  height: 100px;
  width: 250px;
}
.card-user .author .block.block-one {
  background: rgba(225, 78, 202, 0.6);
  background: -webkit-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -o-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -moz-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=10);
  -webkit-transform: rotate(150deg);
  -moz-transform: rotate(150deg);
  -ms-transform: rotate(150deg);
  -o-transform: rotate(150deg);
  transform: rotate(150deg);
  margin-top: -90px;
  margin-left: -50px;
}
.card-user .author .block.block-two {
  background: rgba(225, 78, 202, 0.6);
  background: -webkit-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -o-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -moz-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=10);
  -webkit-transform: rotate(30deg);
  -moz-transform: rotate(30deg);
  -ms-transform: rotate(30deg);
  -o-transform: rotate(30deg);
  transform: rotate(30deg);
  margin-top: -40px;
  margin-left: -100px;
}
.card-user .author .block.block-three {
  background: rgba(225, 78, 202, 0.6);
  background: -webkit-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -o-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -moz-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=10);
  -webkit-transform: rotate(170deg);
  -moz-transform: rotate(170deg);
  -ms-transform: rotate(170deg);
  -o-transform: rotate(170deg);
  transform: rotate(170deg);
  margin-top: -70px;
  right: -45px;
}
.card-user .author .block.block-four {
  background: rgba(225, 78, 202, 0.6);
  background: -webkit-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -o-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: -moz-linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  background: linear-gradient(to right, rgba(225, 78, 202, 0.6) 0%, rgba(225, 78, 202, 0) 100%);
  filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=10);
  -webkit-transform: rotate(150deg);
  -moz-transform: rotate(150deg);
  -ms-transform: rotate(150deg);
  -o-transform: rotate(150deg);
  transform: rotate(150deg);
  margin-top: -25px;
  right: -45px;
}
.card-user .avatar {
  width: 124px;
  height: 124px;
  border: 5px solid #2b3553;
  border-bottom-color: transparent;
  background-color: transparent;
  position: relative;
}
.card-user .card-body {
  min-height: 240px;
}
.card-user hr {
  margin: 5px 15px;
}
.card-user .card-description {
  margin-top: 30px;
}
.title {
  font-weight: 400;
  color: rgba(255, 255, 255, 1);
  font-size: .875rem;
  text-transform: uppercase;
}
.account-number {
  font-size: 1.125rem;
  color: rgba(255, 255, 255, 0.6);
}
.card .balance {
  margin: 25px 0;
  font-size: 1rem;
  color: rgba(255, 255, 255, 0.8);
  position: relative;
}
.card .balance b {
  font-weight: 600;
}
.card .balance span.account-status {
  background: #2da3e0;
  margin-left: 10px;
  margin-top: -1px;
  padding: 2px 6px;
  color: #27293d;
  font-size: .75rem;
  border-radius: .25rem;
  text-transform: uppercase;
  /* position: absolute; */
}
.card .balance span.account-status.active {
  background: #2da3e0;
}
.card .balance span.account-status.inactive {
  background: #e3879e;
}
.card .amount-cont {
  color: rgba(255, 255, 255, 0.6);
  font-size: .875rem;
  font-weight: 300;
}
.form-control {
  background-color: #27293d;
  font-size: 1.5rem;
  font-weight: 300;
  color: #fff;
  border: 1px solid #e14eca;
}
.input-group-text {
  font-size: 1.5rem;
  font-weight: 400;
  line-height: 1.5;
  color: #fff;
  background-color: #27293d;
  border: 1px solid #e14eca;
}
.error-message {
  text-align: center;
}
.text-warning {
  color: #ff8d72 !important;
  font-size: .875rem;
  font-weight: 300;
  margin-bottom: 1px;
}

```

------
### Final Output:

Now run the application and see its working as below :  

![DepositFunds](https://user-images.githubusercontent.com/100709775/185105908-ce8991b3-237e-4af7-80d6-7f241a84566f.gif)




