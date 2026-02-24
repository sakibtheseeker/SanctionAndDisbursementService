using SanctionsAndDisbursementService.Application.DTO.Sanction;
using SanctionsAndDisbursementService.Application.Helper;
using SanctionsAndDisbursementService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.Interfaces
{
    public interface ISanctionRepo
    {
        Task AddSanctions(SanctionDto dto);

        Task<SanctionResponseDto> GetSanctionById(int id);

        Task<List<SanctionResponseDto>> GetAllSanctions();

        Task<SanctionResponseDto> FindSanctions(int id);
    }
}
