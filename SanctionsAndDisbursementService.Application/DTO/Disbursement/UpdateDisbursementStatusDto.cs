using SanctionsAndDisbursementService.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.DTO.Disbursement
{
    public class UpdateDisbursementStatusDto
    {
        public DisbursementStatus Status { get; set; }
    }

}
