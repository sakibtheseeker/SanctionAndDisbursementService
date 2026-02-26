using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.DTO.Sanction
{
    public class SanctionPreviewDto
    {
        public int dealId { get; set; }
        public string customerName { get; set; }
        public decimal eligibleAmount { get; set; }
        public decimal approvedAmount { get; set; }
        public int cibilScore { get; set; }
        public int riskRating { get; set; }
    }
}
