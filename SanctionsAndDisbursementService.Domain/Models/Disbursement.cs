using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SanctionsAndDisbursementService.Domain.Enum;

namespace SanctionsAndDisbursementService.Domain.Models
{
    public class Disbursement
    {
        [Key]
        public int disbursementId { get; set; }
        public int dealId { get; set; }

        public int branchId { get; set; }

        public decimal disburseAmount { get; set; }

        public DateTime disburseDate { get; set; }=DateTime.Now;

        public Guid transactionReference { get; set; } = Guid.NewGuid();

        public DisbursementStatus status { get; set; } = DisbursementStatus.Initiated;

        public bool isActive { get; set; } = true;

        public DateTime createdAt { get; set; } = DateTime.Now;

        public int createdBy { get; set; }

        public DateTime? modifiedAt { get; set; }

        public int? deletedBy { get; set; }

    }
}

