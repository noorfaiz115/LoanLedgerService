using LoanLedgerService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Infrastructure.Services
{
    public class LoanQueryService : ILoanQueryService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanQueryService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<decimal> GetBalanceAsync(long loanId)
        {
            var loan =await _loanRepository.GetByLoanIdAsync(loanId);
            if (loan == null)
            {

                throw new Exception("Loan not found");
            }
            return loan.OutstandingPrincipal;
        }

        public async Task<decimal> GetOutstandingAsync(long loanId)
        {
            var loan = await _loanRepository.GetByLoanIdAsync(loanId);
            if (loan == null)
            {

                throw new Exception("Loan not found");
            }
            return loan.OutstandingPrincipal -loan.OutstandingInterest;
        }
    }
}
