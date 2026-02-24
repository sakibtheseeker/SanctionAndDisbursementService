using SanctionsAndDisbursementService.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.DTO.Disbursement
{
    public class DisbursementResponseDto
    {
        public int disbursementId { get; set; }
        public int dealId { get; set; }

        public int branchId { get; set; }

        public double disburseAmount { get; set; }

        public DateTime disburseDate { get; set; } = DateTime.Now;

        public Guid transactionReference { get; set; } = Guid.NewGuid();

        public DisbursementStatus status { get; set; } = DisbursementStatus.Initiated;

        public string loanTypeName { get; set; }
        public decimal eligibleAmount { get; set; }
        public decimal approvedAmount { get; set; }
        public int riskRating { get; set; }
        public int cibilScore { get; set; }
    }
}
