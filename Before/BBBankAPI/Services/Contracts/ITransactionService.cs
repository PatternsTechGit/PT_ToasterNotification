using Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ITransactionService
    {
        Task<LineGraphData> GetLast12MonthBalances(string? userId);
    }
}
