using SanctionsAndDisbursementService.Application.DTO.LoanDeals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.DTO.Sanction
{
    public class SanctionResponseDto
    {
        public int sanctionId { get; set; }

        public int dealId { get; set; }

        public Guid sanctionNo { get; set; } = Guid.NewGuid();

        public decimal loanAmount { get; set; }

        public decimal interestRate { get; set; }

        public int tenureMonths { get; set; }

        public decimal emiAmount { get; set; }

        public string repaymentSchedule { get; set; }

        public string? pdfPath { get; set; }

        public string loanTypeName { get; set; }
        public decimal eligibleAmount { get; set; }
        public decimal approvedAmount { get; set; }
        public int riskRating { get; set; }
        public int cibilScore { get; set; }

        public string currentStatus { get; set; }

    }
}
