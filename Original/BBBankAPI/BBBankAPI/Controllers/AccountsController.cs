using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace BBBankAPI.Controllers
{
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
}
