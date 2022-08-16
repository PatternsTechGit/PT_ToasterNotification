using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Request
{
    public class DepositRequest
    {
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
