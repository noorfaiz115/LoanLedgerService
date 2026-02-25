using CryptoHub.Application.Common;
using LoanLedgerService.Application.DTOs;
using LoanLedgerService.Application.Interfaces;
using LoanLedgerService.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedgerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanAccountController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILoanQueryService _loanQueryService;

        public LoanAccountController(ILoanRepository loanRepository, ILoanQueryService loanQueryService)
        {
            _loanRepository = loanRepository;
            _loanQueryService = loanQueryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortedBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var loan = await _loanRepository.GetAllAsync(filterOn, filterQuery, sortedBy, isAscending ?? true, pageNumber, pageSize);
            if (loan == null)
            {

                return NotFound("Not Found");
            }

            var response = ApiResponse<List<LoanAccountResponse>>.SuccessResponse(loan, "Fetch Successfully");
            return Ok(response);
        }
       
        [HttpGet("{loanId:long}")]
        public async Task<IActionResult> GetByLoanId(int loanId)
        {
            var loan = await _loanRepository.GetByLoanIdAsync(loanId);
            if (loan == null)
            {

                return NotFound("Not Found");
            }

            var response = ApiResponse<LoanAccountResponse>.SuccessResponse(loan, "Fetch Successfully");
            return Ok(response);
        }

        [HttpDelete("{loanId:long}")]
        public async Task<IActionResult> Delete(long loanId, DeleteLoanAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something went wrong while deleting");
            }

            var isDeleted =await _loanRepository.SoftDeleteAsync(loanId, dto.DeletedBy);

            if (!isDeleted)
            {
                return NotFound();
            }
            var response = ApiResponse<bool>.SuccessResponse(true, "Loan deleted successfully");

            return Ok(response);
        }


        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(long customerId)
        {
            var Customerloan = await _loanRepository.GetByCustomerIdAsync(customerId);
            if (Customerloan == null)
            {

                return NotFound("Not Found");
            }

            var response = ApiResponse<LoanAccountResponse>.SuccessResponse(Customerloan, "Fetch Successfully");
            return Ok(response);
        }


        [HttpGet("branch/{branchId:long}")]
        public async Task<IActionResult> GetByBranch(long branchId, [FromQuery] AccountStatus? status = null)
        {
            var result = await _loanRepository.GetByBranchAsync(branchId, status);
            var response = ApiResponse<IEnumerable<LoanAccountSummaryDto>>.SuccessResponse(result, "Done");
            return Ok(response);
        }

        [HttpGet("sanction/{sanctionNo}")]
        public async Task<IActionResult> GetBySanctionNo(string sanctionNo)
        {
            var result = await _loanRepository.GetBySanctionNoAsync(sanctionNo);
            if (result == null)
                throw new Exception("Data Not Found");

            return Ok(ApiResponse<LoanAccountResponse>.SuccessResponse(result,"Done"));
        }




        [HttpGet("/{loanTypeId}")]
        public async Task<IActionResult> GetByLoanTypeId(int loanTypeId)
        {
            var loan = await _loanRepository.GetByLoanTypeIdAsync(loanTypeId);
            if (loan == null)
            {

                return NotFound("Not Found");
            }

            var response = ApiResponse<LoanAccountResponse>.SuccessResponse(loan, "Fetch Successfully");
            return Ok(response);
        }

       

        [HttpPost("CreateLoan")]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanAccountDto createLoanAccountDto)
        {
            var loanAccount = await _loanRepository.CreateAsync(createLoanAccountDto);


            var response = ApiResponse<LoanAccountResponse>.SuccessResponse(loanAccount, "Loan Account Created successfully ");
            return Ok(response);
        }

        [HttpGet("{loanId}/balance")]
        public async Task<IActionResult> GetBalance(int loanId)
        {

            var balance = await _loanQueryService.GetBalanceAsync(loanId);
            var response = ApiResponse<decimal>.SuccessResponse(balance, "Balance Fetch");
            return Ok(response);
        }

        [HttpGet("{loanId}/outstanding")]
        public async Task<IActionResult> GetOutstanding(long loanId)
        {
            var outstanding = await _loanQueryService.GetOutstandingAsync(loanId);
            return Ok(outstanding);
        }

    }
}
