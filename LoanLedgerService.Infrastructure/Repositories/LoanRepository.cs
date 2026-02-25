using AutoMapper;
using LoanLedgerService.Application.DTOs;
using LoanLedgerService.Application.Interfaces;
using LoanLedgerService.Application.Services;
using LoanLedgerService.Domain.Models;
using LoanLedgerService.Infrastructure.Data;
using LoanLedgerService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LoanLedgerService.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IEmiServiceClient _emiServiceClient;

        public LoanRepository(IMapper mapper, ApplicationDbContext context, IEmiServiceClient emiServiceClient)
        {
            _mapper = mapper;
            _context = context;
            _emiServiceClient = emiServiceClient;
        }

        public async Task<LoanAccountResponse> CreateAsync(CreateLoanAccountDto createLoanAccountDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                long dealId = createLoanAccountDto.DealId;
                long sanctionId = createLoanAccountDto.SanctionId; ;
                long disbursementId = createLoanAccountDto.DisbursementId;
                long customerId = createLoanAccountDto.CustomerId;
                long loanTypeId = createLoanAccountDto.LoanTypeId;
                long branchId = createLoanAccountDto.BranchId;

                var loan = _mapper.Map<LoanAccount>(createLoanAccountDto);
                loan.DealId = dealId;
                loan.SanctionId = sanctionId;
                loan.DisbursementId = disbursementId;
                loan.CustomerId = customerId;
                loan.LoanTypeId = loanTypeId;
                loan.BranchId = branchId;


                loan.SanctionNo = createLoanAccountDto.SanctionNo;
                loan.SanctionedAmount = createLoanAccountDto.SanctionedAmount;
                loan.InterestRate = createLoanAccountDto.InterestRate;
                loan.TenureMonths = createLoanAccountDto.TenureMonths;
                loan.EmiAmount = createLoanAccountDto.EmiAmount;

                loan.DisbursedAmount = createLoanAccountDto.DisbursedAmount;
                loan.DisbursementDate = createLoanAccountDto.DisbursementDate;
                loan.CreatedAt = DateTime.UtcNow;
                loan.AccountStatus = AccountStatus.Active;


                loan.OutstandingPrincipal = loan.DisbursedAmount;
                loan.FirstEmiDate = loan.DisbursementDate.AddDays(30);
                loan.EmiDay = loan.FirstEmiDate.Day;
                loan.MaturityDate = loan.DisbursementDate.AddMonths(loan.TenureMonths);
                loan.NextEmiDate = loan.FirstEmiDate;
                loan.RemainingTenure = loan.TenureMonths;
                loan.NextEmiDate = loan.FirstEmiDate;


                _context.LoanAccounts.Add(loan);
                await _context.SaveChangesAsync();

                await _emiServiceClient.CreateEmiScheduleAsync(new CreateEmiScheduleRequest
                {
                    LoanId = loan.LoanId,
                    LoanAmount = loan.OutstandingPrincipal,
                    InterestRate = loan.InterestRate,
                    TenureMonths = loan.TenureMonths,
                    EmiAmount = loan.EmiAmount,
                    DisbursementDate = DateTime.UtcNow
                });

                await transaction.CommitAsync();




                var loanAccount = _mapper.Map<LoanAccountResponse>(loan);
                return loanAccount;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }


        }

        public async Task<List<LoanAccountResponse>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortedBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10)
        {
            var loan = _context.LoanAccounts.Where(q => q.DeletedAt == null).AsQueryable();

            //Filter 
            if (string.IsNullOrEmpty(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("SanctionNo", StringComparison.OrdinalIgnoreCase))
                {

                    loan = loan.Where(x => x.SanctionNo.Contains(filterQuery));
                }
            }
            //sorting
            if (string.IsNullOrEmpty(sortedBy) == false)
            {
                if (sortedBy.Equals("DisbursedAmount", StringComparison.OrdinalIgnoreCase))
                {
                    loan = isAscending ? loan.OrderBy(x => x.DisbursedAmount) : loan.OrderByDescending(x => x.DisbursedAmount);
                }
                else if (sortedBy.Equals("NextEmiDate", StringComparison.OrdinalIgnoreCase))
                {

                    loan = isAscending ? loan.OrderBy(x => x.NextEmiDate) : loan.OrderByDescending(x => x.NextEmiDate);
                }
            }

            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;
            var loanData = _mapper.Map<List<LoanAccountResponse>>(loan);
            return loanData.Skip(skipResult).Take(pageSize).ToList();


        }

        public async Task<LoanAccountResponse> GetByLoanTypeIdAsync(long id)
        {
            var loan = await _context.LoanAccounts.FirstOrDefaultAsync(q => q.LoanTypeId == id && q.AccountStatus == AccountStatus.Active);
            var loanData = _mapper.Map<LoanAccountResponse>(loan);
            return loanData;
        }

        public async Task<LoanAccountResponse> GetByLoanIdAsync(long id)
        {
            var loan = await _context.LoanAccounts.FirstOrDefaultAsync(q => q.LoanId == id && q.AccountStatus == AccountStatus.Active);
            var loanData = _mapper.Map<LoanAccountResponse>(loan);
            return loanData;
        }

        public async Task<LoanAccountResponse> GetByCustomerIdAsync(long id)
        {
            var loan = await _context.LoanAccounts.FirstOrDefaultAsync(q => q.CustomerId == id && q.AccountStatus == AccountStatus.Active);
            var loanData = _mapper.Map<LoanAccountResponse>(loan);
            return loanData;
        }

        public async Task<bool> SoftDeleteAsync(long loanId, string deletedBy)
        {
            var loan = await _context.LoanAccounts.FirstOrDefaultAsync(q => q.LoanId == loanId && q.AccountStatus == AccountStatus.Active);
            if (loan == null) return false;

            if (loan.AccountStatus != AccountStatus.Closed && loan.AccountStatus != AccountStatus.WrittenOff)
                throw new InvalidOperationException("Only Closed or WrittenOff accounts can be deleted.");

            loan.DeletedAt = DateTime.UtcNow;
            loan.DeletedBy = deletedBy;
            loan.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<LoanAccountSummaryDto>> GetByBranchAsync(long branchId , AccountStatus? status)
        {
            var loan = _context.LoanAccounts.Where(q => q.BranchId == branchId && q.DeletedAt == null);

            if (status.HasValue)
            {
                loan = loan.Where(q => q.AccountStatus == status.Value);
            }

            var data = _mapper.Map<List<LoanAccountSummaryDto>>(loan);
            return data;
        }

        public async Task<LoanAccountResponse> GetBySanctionNoAsync(string sanctionNo)
        {
            var loan = await _context.LoanAccounts
                .FirstOrDefaultAsync(x => x.SanctionNo == sanctionNo && x.DeletedAt == null);

            if(loan == null)
            {
                return null;
            }

            var data = _mapper.Map<LoanAccountResponse>(loan);
            return data;
        }
    }
}
