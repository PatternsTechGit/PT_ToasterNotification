using Entities.Responses;
using Infrastructure;
using Services.Contracts;

namespace Services
{
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
}