using LoanLedgerService.Application.DTOs;
using LoanLedgerService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Application.Interfaces
{
    public interface ILoanRepository
    {
        Task<LoanAccountResponse> CreateAsync(CreateLoanAccountDto createLoanAccountDto);

        Task<List<LoanAccountResponse>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortedBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10);
        Task<LoanAccountResponse> GetByLoanIdAsync(long id);
        Task<LoanAccountResponse> GetByLoanTypeIdAsync(long id);
        Task<LoanAccountResponse> GetByCustomerIdAsync(long id);


        Task<bool> SoftDeleteAsync(long loanId, string deletedBy);
        Task<IEnumerable<LoanAccountSummaryDto>> GetByBranchAsync(long loanId, AccountStatus? status);
        Task<LoanAccountResponse> GetBySanctionNoAsync(string sanctionNo);
    }
}
