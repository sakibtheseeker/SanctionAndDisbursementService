using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.DTO.Sanction
{
   public class SanctionDto
    {
        public int dealId { get; set; }

        public decimal interestRate { get; set; }

        public int tenureMonths { get; set; }


    }
}
