using SanctionsAndDisbursementService.Application.DTO.Disbursement;
using SanctionsAndDisbursementService.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.Interfaces
{
    public interface IDisbursementRepo
    {
        void AddDisbursement(DisbursementDto dto);

        Task<List<DisbursementResponseDto>> GetAllDisbursement();

        Task<DisbursementResponseDto> GetDisbursementById(int id);

        public void CompleteDisbursement(int id, UpdateDisbursementStatusDto dto);
    }
}
