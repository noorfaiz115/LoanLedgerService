using LoanLedgerService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Application.Services
{
    public interface IEmiServiceClient
    {
         Task CreateEmiScheduleAsync(CreateEmiScheduleRequest request);
    }
}
