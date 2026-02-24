using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using SanctionsAndDisbursementService.Application.DTO.Disbursement;
using SanctionsAndDisbursementService.Application.DTO.LoanDeals;
using SanctionsAndDisbursementService.Application.Helper;
using SanctionsAndDisbursementService.Application.Interfaces;
using SanctionsAndDisbursementService.Domain.Enum;
using SanctionsAndDisbursementService.Domain.Models;
using SanctionsAndDisbursementService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QuestPDF.Helpers.Colors;

namespace SanctionsAndDisbursementService.Infrastructure.Repository
{
    public class DisbursementRepo : IDisbursementRepo
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly LoanDealsClient client;
        public DisbursementRepo(ApplicationDbContext db, IMapper mapper, LoanDealsClient client)
        {
            this.db = db;
            this.mapper = mapper;
            this.client = client;
        }

        public void AddDisbursement(DisbursementDto dto)
        {
            if (dto.dealId <= 0)
                throw new ArgumentException("Invalid Deal Id Provided");

            if (dto.branchId <= 0)
                throw new ArgumentException("Invalid Branch Id Provided");

            var sanction = db.Sanctions
                .FirstOrDefault(x => x.dealId == dto.dealId);

            if (sanction == null)
                throw new Exception("No sanction found for this deal");

            decimal sanctionedAmount = sanction.loanAmount;

            var disbursement = mapper.Map<Disbursement>(dto);

            disbursement.disburseAmount = sanctionedAmount;

            db.Disbursements.Add(disbursement);
            db.SaveChanges();
        }


        public async Task<List<DisbursementResponseDto>> GetAllDisbursement()
        {

            var d = await db.Disbursements.ToListAsync();
            
            var res = mapper.Map<List<DisbursementResponseDto>>(d);
            
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
                }
            }
            return res;
        }

        public async Task<DisbursementResponseDto> GetDisbursementById(int id)
        {
            var d = await db.Disbursements.FindAsync(id);

            if (d == null)
            {
                throw new Exception("Data Not Found");
            }

            var res = mapper.Map<DisbursementResponseDto>(d);

            LoanDealsDto deal = await client.GetLoanDealsById(d.dealId);
            if (deal != null)
    {
                res.loanTypeName = deal.loanTypeName;
                res.eligibleAmount = deal.eligibleAmount;
                res.approvedAmount = deal.approvedAmount;
                res.riskRating = deal.riskRating;
                res.cibilScore = deal.cibilScore;
            }
            return res;
        }

        public void CompleteDisbursement(int id, UpdateDisbursementStatusDto dto)
        {
            var disbursement = db.Disbursements
                .FirstOrDefault(x => x.disbursementId == id);

            if (disbursement == null)
                throw new KeyNotFoundException("Disbursement not found");

            if (disbursement.status != DisbursementStatus.Initiated)
                throw new InvalidOperationException("Only initiated disbursement can be updated");

            if (dto.Status != DisbursementStatus.Success &&
                dto.Status != DisbursementStatus.Failed)
                throw new ArgumentException("Status must be Success or Failed");

            disbursement.status = dto.Status;
            disbursement.modifiedAt = DateTime.Now;

            db.SaveChanges();
        }

    }
}
