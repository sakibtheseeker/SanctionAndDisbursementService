using AutoMapper;
using SanctionsAndDisbursementService.Application.DTO.Disbursement;
using SanctionsAndDisbursementService.Application.DTO.Sanction;
using SanctionsAndDisbursementService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.Mapper
{
    public class MapperConfig: Profile
    {
        public MapperConfig()
        {
            CreateMap<SanctionDto, Sanction>().ReverseMap();
            CreateMap<SanctionResponseDto, Sanction>().ReverseMap();
            CreateMap<DisbursementDto, Disbursement>().ReverseMap();
            CreateMap<DisbursementResponseDto,Disbursement>().ReverseMap();
            

        }
    }
}
