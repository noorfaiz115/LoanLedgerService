using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Application.DTOs
{
    public class CreateLoanAccountDto
    {

        [Required]
        public long DealId { get; set; }

        [Required]
        public long SanctionId { get; set; }

        [Required]
        public long DisbursementId { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public long? ScorecardId { get; set; }

        [Required]
        public long LoanTypeId { get; set; }

        [Required]
        public long BranchId { get; set; }


        [Required]
        [MaxLength(50)]
        public string SanctionNo { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SanctionedAmount { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; }

        public int TenureMonths { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EmiAmount { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal DisbursedAmount { get; set; }

        public DateTime DisbursementDate { get; set; }

        [MaxLength(100)]
        public string? TransactionReference { get; set; }
    

    }
}
