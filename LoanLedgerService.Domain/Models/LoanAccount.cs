using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Domain.Models
{
    public class LoanAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LoanId { get; set; }

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


        public int EmiDay { get; set; }

        public DateTime MaturityDate { get; set; }

        public DateTime FirstEmiDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OutstandingPrincipal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OutstandingInterest { get; set; }

        [NotMapped]
        public decimal TotalOutstanding => OutstandingPrincipal + OutstandingInterest;

        public int RemainingTenure { get; set; }

        public int Dpd { get; set; }

        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        public DateTime? LastPaymentDate { get; set; }

        public DateTime? NextEmiDate { get; set; }


        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? DeletedBy { get; set; }


        

    }


    public enum AccountStatus
    {
        Active = 1,
        Closed = 2,
        Foreclosed = 3,
        WrittenOff = 4
    }       
}
