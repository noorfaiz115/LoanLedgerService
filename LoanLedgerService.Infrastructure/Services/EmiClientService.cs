using LoanLedgerService.Application.DTOs;
using LoanLedgerService.Application.Services;
using LoanLedgerService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Infrastructure.Services
{
    public class EmiClientService :IEmiServiceClient
    {
        private readonly HttpClient _httpClient;

        public EmiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateEmiScheduleAsync(CreateEmiScheduleRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("generate", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to generate EMI schedule");
            }
        }
    }
}
