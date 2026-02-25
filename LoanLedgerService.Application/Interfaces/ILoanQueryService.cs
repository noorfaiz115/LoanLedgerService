using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Application.Interfaces
{
    public interface ILoanQueryService
    {

        Task<decimal> GetBalanceAsync(long loanId);
        Task<decimal> GetOutstandingAsync(long loanId);
    }
}
