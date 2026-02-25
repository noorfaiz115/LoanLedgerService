using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLedgerService.Application.DTOs
{
    public class DeleteLoanAccountDto
    {
            [Required]
            [MaxLength(100)]
            public string DeletedBy { get; set; } = string.Empty;
        
    }
}
