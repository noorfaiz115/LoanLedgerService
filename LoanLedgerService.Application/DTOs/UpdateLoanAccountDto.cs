using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Application.DTOs
{
    public class UpdateLoanAccountDto
    {
        [Required] public long BranchId { get; set; }
        public long? ScorecardId { get; set; }

        [MaxLength(100)]
        public string? TransactionReference { get; set; }

        [Range(1, 28, ErrorMessage = "EmiDay must be between 1 and 28.")]
        public int EmiDay { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }
    }
}
