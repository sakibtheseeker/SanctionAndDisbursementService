using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Domain.Models
{
    public class Sanction
    {
        [Key]
        public int sanctionId { get; set; }

        public int dealId { get; set; }

        public Guid sanctionNo { get; set; }=Guid.NewGuid();

        public decimal loanAmount { get; set; }

        public decimal interestRate { get; set; }

        public int tenureMonths { get; set; }

        public decimal emiAmount { get; set; }

        public string repaymentSchedule { get; set; } = "Monthly";

        public bool customerAccepted { get; set; } = false;

        public DateTime? acceptedDate { get; set; }

        public string? pdfPath { get; set; }

        public bool isActive { get; set; } = true;

        public DateTime createdAt { get; set; } = DateTime.Now;

        public int createdBy { get; set; }

        public DateTime? modifiedAt { get; set; }

        public int? deletedBy { get; set; }
    }
}
