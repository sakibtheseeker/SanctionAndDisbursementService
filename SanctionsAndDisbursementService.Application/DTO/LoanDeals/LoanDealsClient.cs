using SanctionsAndDisbursementService.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SanctionsAndDisbursementService.Application.DTO.LoanDeals
{
    public class LoanDealsClient
    {
        HttpClient client;
        public LoanDealsClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<LoanDealsDto?> GetLoanDealsById(int id)
        {
            var response = await client
                .GetFromJsonAsync<ApiResponse<LoanDealsDto>>($"api/LoanDeals/{id}");

            return response?.Data;
        }


    }
}
