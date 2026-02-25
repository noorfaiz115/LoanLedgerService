using AutoMapper;
using LoanLedgerService.Application.DTOs;
using LoanLedgerService.Domain.Models;

namespace LoanLedgerService.API.Middleware
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<LoanAccount, LoanAccountResponse>().ReverseMap();
            CreateMap<LoanAccount, CreateLoanAccountDto>().ReverseMap();

            CreateMap<LoanAccount, LoanAccountSummaryDto>().ReverseMap();
        }
    }
}
