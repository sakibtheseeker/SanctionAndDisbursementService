using Microsoft.EntityFrameworkCore;
using SanctionsAndDisbursementService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
      
        public DbSet<Sanction> Sanctions { get; set; }
        public DbSet<Disbursement> Disbursements { get; set; }

    }
}


