using Entities;

namespace Infrastructure
{
    public class BBBankContext
    {
        private string AzureADUserID = "aa45e3c9-261d-41fe-a1b0-5b4dcf79cfd3";
        public BBBankContext()
        {
            // creating the collection for user list
            this.Users = new List<User>();

            // initializing a new user 
            this.Users.Add(new User
            {
                Id = AzureADUserID,
                FirstName = "Raas",
                LastName = "Masood",
                Email = "rassmasood@hotmail.com",
               // ProfilePicUrl = "https://res.cloudinary.com/demo/image/upload/w_400,h_400,c_crop,g_face,r_max/w_200/lady.jpg"
            ProfilePicUrl= "https://res.cloudinary.com/dzlqlwffb/image/upload/v1660734804/1_ljnh0m.webp"
            });

            // creating the collection for account list
            this.Accounts = new List<Account>();

            // initializing a new account 
            this.Accounts.Add(new Account
            {
                Id = "aa45e3c9-261d-41fe-a1b0-5b4dcf79cfd3",
                AccountNumber = "0001-1001",
                AccountTitle = "Raas Masood",
                CurrentBalance = 3500M,
                AccountStatus = AccountStatus.Active,
                User = this.Users[0]
            });

            // creating the collection for transaction list
            this.Transactions = new List<Transaction>();

            // initializing with some transactions 
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                TransactionAmount = 1000M,
                TransactionDate = DateTime.Now,
                TransactionType = TransactionType.Deposit,
                Account = this.Accounts[0]
            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                TransactionAmount = -100M,
                TransactionDate = DateTime.Now.AddMonths(-1),
                TransactionType = TransactionType.Withdraw,
                Account = this.Accounts[0]
            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                TransactionAmount = -45M,
                TransactionDate = DateTime.Now.AddMonths(-2),
                TransactionType = TransactionType.Withdraw,
                Account = this.Accounts[0]
            });
            this.Transactions.Add(new Transaction()
            {


                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = -200M,
                TransactionDate = DateTime.Now.AddMonths(-4),
                TransactionType = TransactionType.Withdraw

            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = 500M,
                TransactionDate = DateTime.Now.AddMonths(-5),
                TransactionType = TransactionType.Deposit

            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = 200M,
                TransactionDate = DateTime.Now.AddMonths(-6),
                TransactionType = TransactionType.Deposit

            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = -300M,
                TransactionDate = DateTime.Now.AddMonths(-7),
                TransactionType = TransactionType.Withdraw

            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = -100M,
                TransactionDate = DateTime.Now.AddMonths(-8),
                TransactionType = TransactionType.Withdraw

            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = 200M,
                TransactionDate = DateTime.Now.AddMonths(-9),
                TransactionType = TransactionType.Deposit

            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = -500M,
                TransactionDate = DateTime.Now.AddMonths(-10),
                TransactionType = TransactionType.Withdraw

            });
            this.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Account = this.Accounts[0],
                TransactionAmount = 900M,
                TransactionDate = DateTime.Now.AddMonths(-11),
                TransactionType = TransactionType.Deposit

            });
        }

        public List<Transaction> Transactions { get; set; }
        public List<Account> Accounts { get; set; }
        public List<User> Users { get; set; }
    }
}