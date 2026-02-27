using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SanctionsAndDisbursementService.Application.DTO.LoanDeals;
using SanctionsAndDisbursementService.Application.DTO.Sanction;
using SanctionsAndDisbursementService.Application.Helper;
using SanctionsAndDisbursementService.Application.Interfaces;
using SanctionsAndDisbursementService.Domain.Models;
using SanctionsAndDisbursementService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QuestPDF.Helpers.Colors;

namespace SanctionsAndDisbursementService.Infrastructure.Repository
{
    public class SanctionRepo : ISanctionRepo
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly LoanDealsClient client;
        public SanctionRepo(ApplicationDbContext db, IMapper mapper, LoanDealsClient client)
        {
            this.db = db;
            this.mapper = mapper;
            this.client = client;
        }

        public async Task AddSanctions(SanctionDto dto)
        {

            if (dto.dealId <= 0)
            {
                throw new ArgumentException("Invalid Deal Id Provided");
            }


            var existingSanction = await db.Sanctions
    .FirstOrDefaultAsync(x => x.dealId == dto.dealId);

            if (existingSanction != null)
            {
                throw new Exception("Sanction already exists for this Deal");
            }

            var deal = await client.GetLoanDealsById(dto.dealId);

            if (deal == null)
                throw new Exception("Loan Deal not found");

            if (dto.interestRate <= 0 || dto.interestRate > 40)
            {
                throw new ArgumentException("Invalid Interest Rate");
            }

            if (dto.tenureMonths <= 0)
            {
                throw new ArgumentException("Tenure months should be greater than zero");
            }

            if (deal.currentStatus != "Approved")
                throw new Exception("Loan Deal must be Approved before sanctioning");


            decimal principal = (decimal)deal.approvedAmount;

            if (principal <= 0)
                throw new Exception("Approved amount must be greater than zero");

            if (dto.interestRate <= 0 || dto.interestRate > 100)
                throw new ArgumentException("Invalid Interest Rate");

            if (dto.tenureMonths <= 0)
                throw new ArgumentException("Tenure months should be greater than zero");

            decimal annualRate = dto.interestRate;
            int tenure = dto.tenureMonths;

            decimal monthlyRate = annualRate / 12 / 100;
            decimal emi;

            if (monthlyRate == 0)
            {
                emi = principal / tenure;
            }
            else
            {
                double pow = Math.Pow((double)(1 + monthlyRate), tenure);

                emi = principal * monthlyRate * (decimal)pow
                      / ((decimal)pow - 1);
            }

            emi = Math.Round(emi, 2);

            var sanction = mapper.Map<Sanction>(dto);

            sanction.loanAmount = principal;
            sanction.emiAmount = emi;

            db.Sanctions.Add(sanction);
            await db.SaveChangesAsync();


            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(1, Unit.Inch);
                    page.Header().Text($"Sanction Report #{sanction.sanctionId}")
                                 .FontSize(20);

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Loan Amount: {sanction.loanAmount}");
                        col.Item().Text($"Interest Rate: {sanction.interestRate}%");
                        col.Item().Text($"Tenure: {sanction.tenureMonths} months");
                        col.Item().Text($"EMI Amount: {sanction.emiAmount}");
                        col.Item().Text($"Repayment Schedule: {sanction.repaymentSchedule}");
                    });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();

            string folderName = "SanctionPdfs";
            string fileName = $"Sanction_{sanction.sanctionId}.pdf";

            string physicalFolderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                folderName
            );

            if (!Directory.Exists(physicalFolderPath))
                Directory.CreateDirectory(physicalFolderPath);

            string physicalFullPath = Path.Combine(physicalFolderPath, fileName);

            File.WriteAllBytes(physicalFullPath, pdfBytes);


            sanction.pdfPath = Path.Combine(folderName, fileName)
                                    .Replace("\\", "/");

            await db.SaveChangesAsync();


        }

        public async Task<SanctionResponseDto> FindSanctions(int id)
        {
            var d = await db.Sanctions.FirstOrDefaultAsync(c => c.sanctionId == id && c.isActive);
            var res = mapper.Map<SanctionResponseDto>(d);
            return res;
        }

        public async Task<List<SanctionResponseDto>> GetAllSanctions()
        {
            var d = await db.Sanctions.ToListAsync();

            var res = mapper.Map<List<SanctionResponseDto>>(d);
            foreach (var i in res)
            {
                LoanDealsDto deal = await client.GetLoanDealsById(i.dealId);
                if (deal != null)
                {
                    i.loanTypeName = deal.loanTypeName;
                    i.eligibleAmount = deal.eligibleAmount;
                    i.approvedAmount = deal.approvedAmount;
                    i.riskRating = deal.riskRating;
                    i.cibilScore = deal.cibilScore;
                    i.authUserName = deal.authUserName;
                    i.currentStatus = deal.currentStatus;
                }
            }
            return res;
        }

        public async Task<SanctionResponseDto> GetSanctionById(int id)
        {
            var d = await db.Sanctions.FindAsync(id);

            if (d == null)
                return null;

            LoanDealsDto deal = await client.GetLoanDealsById(d.dealId); 

            var res = mapper.Map<SanctionResponseDto>(d);

            if (deal != null)
            {
                res.loanTypeName = deal.loanTypeName;
                res.eligibleAmount = deal.eligibleAmount;
                res.approvedAmount = deal.approvedAmount;
                res.riskRating = deal.riskRating;
                res.cibilScore = deal.cibilScore;

                res.authUserName = deal.authUserName;
                res.currentStatus = deal.currentStatus;
            }

            return res;
        }

        public async Task<SanctionPreviewDto> GetSanctionPreview(int dealId)
        {
            var deal = await client.GetLoanDealsById(dealId);

            if (deal == null)
                throw new Exception("Loan Deal not found");

            return new SanctionPreviewDto
            {
                dealId = deal.dealId,
                customerName = deal.authUserName,   
                eligibleAmount = deal.eligibleAmount,
                approvedAmount = deal.approvedAmount,
                cibilScore = deal.cibilScore,
                riskRating = deal.riskRating
            };
        }
    }
}
