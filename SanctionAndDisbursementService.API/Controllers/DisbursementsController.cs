using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanctionsAndDisbursementService.Application.DTO.Disbursement;
using SanctionsAndDisbursementService.Application.DTO.Sanction;
using SanctionsAndDisbursementService.Application.Helper;
using SanctionsAndDisbursementService.Application.Interfaces;

namespace SanctionAndDisbursementService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisbursementsController : ControllerBase
    {
        private readonly IDisbursementRepo repo;

        public DisbursementsController(IDisbursementRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllDisbursements()
        {
            var data = await repo.GetAllDisbursement();

            if (data != null)
            {
                return Ok(ApiResponse<List<DisbursementResponseDto>>.SuccessResponse(
                data, "Disbursement Fetched Successfully"));
            }

            return Ok(ApiResponse<SanctionResponseDto>.FailureResponse(
          "Data Unavailable", "No records found.", "404"));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FetchAllDisbursementById(int id)
        {
            var data = await repo.GetDisbursementById(id);

            if(data!=null)
            {
                return Ok(ApiResponse<DisbursementResponseDto>.SuccessResponse(
                data, "Disbursement Fetched Successfully"));
            }
            
            return Ok(ApiResponse<SanctionResponseDto>.FailureResponse(
              "Data Unavailable", "No records found.", "404"));
        }

        [HttpPost]
        public IActionResult AddDisbursement(DisbursementDto dto)
        {
            repo.AddDisbursement(dto);
            return Ok(ApiResponse<string>.SuccessResponse(
            null, "Disbursement added Successfully"));
        }

        [HttpPut("/{id}/complete")]
        public IActionResult ChangeStatus(int id, UpdateDisbursementStatusDto dto)
        {
            repo.CompleteDisbursement(id,dto);
            return Ok(ApiResponse<string>.SuccessResponse(
            null,
            $"Disbursement marked as {dto.Status}"));
        }

    }
}
