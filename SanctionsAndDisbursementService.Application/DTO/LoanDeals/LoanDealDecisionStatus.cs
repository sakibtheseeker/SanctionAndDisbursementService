using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.DTO.LoanDeals
{
    public enum LoanDealDecisionStatus
    {
        Review = 1,
        Approved = 2,
        Rejected = 3,
        Sanctioned = 4
    }
}
