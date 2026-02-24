using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanctionsAndDisbursementService.Application.DTO.Sanction;
using SanctionsAndDisbursementService.Application.Helper;
using SanctionsAndDisbursementService.Application.Interfaces;
using System.Reflection.Metadata;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SanctionAndDisbursementService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanctionsController : ControllerBase
    {
        private readonly ISanctionRepo repo;

        public SanctionsController(ISanctionRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllSanctions()
        {
            var data = await repo.GetAllSanctions();
            if(data!=null)
            {
                return Ok(ApiResponse<List<SanctionResponseDto>>.SuccessResponse(
               data, "Sanctions Fetched Successfully"));
            }

            return Ok(ApiResponse<SanctionResponseDto>.FailureResponse(
              "Data Unavailable", "No records found.", "404"));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FetchSanctionsById(int id)
        {
            var data = await repo.GetSanctionById(id);
            if(data!=null)
            {
                return Ok(ApiResponse<SanctionResponseDto>.SuccessResponse(
               data, "Sanctions Fetched Successfully"));
            }

            return Ok(ApiResponse<SanctionResponseDto>.FailureResponse(
               "Data Unavailable", "No records found.", "404"));
        }

        [HttpPost]
        public async Task <IActionResult> AddSanctions(SanctionDto dto)
        {
            await repo.AddSanctions(dto);

            return Ok(ApiResponse<SanctionResponseDto>.SuccessResponse(
               null, "Sanctions Added Successfully"));
        }


        [HttpGet("sanctions/{id}/pdf")]
        public async Task<IActionResult> GetSanctionPdf(int id)
        {
            var sanction = await repo.FindSanctions(id);

            if (sanction == null || string.IsNullOrEmpty(sanction.pdfPath))
                return NotFound(ApiResponse<string>
                    .FailureResponse("PDF Not Found",
                                     $"No PDF found for sanction id {id}",
                                     "NOT_FOUND"));

            if (!System.IO.File.Exists(sanction.pdfPath))
                return NotFound(ApiResponse<string>
                    .FailureResponse("File Missing",
                                     "PDF file does not exist on server",
                                     "FILE_MISSING"));

            var bytes = await System.IO.File.ReadAllBytesAsync(sanction.pdfPath);

            return File(bytes, "application/pdf",
                        Path.GetFileName(sanction.pdfPath));
        }


    }
}
