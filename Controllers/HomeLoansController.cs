using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BFGBlazor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GraphQL;
using GraphQL.Types;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BFGBlazor.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeLoansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HomeLoansController(ApplicationDbContext context)
        {
            this._context = context;
        }
        // GET: api/<HomeLoansController>
        [HttpGet]
        public PagedResult<HomeLoans> Get([FromQuery] int page)
        {
            int pageSize = 20;
            return _context.HomeLoans.Include(m => m.Residential).OrderBy(p => p.Id).AsNoTracking().GetPaged(page, pageSize);

        }

        // GET api/<HomeLoansController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HomeLoans homeLoan = await _context.HomeLoans
                        .Include(m => m.Residential)
                        .SingleOrDefaultAsync(m => m.Id == id);


            if (homeLoan == null)
            {
                return NotFound();
            }

            return Ok(homeLoan);
        }


        // GET /api/HomeLoans/SyncDatabase/
        [Route("SyncDatabase")]
        public async Task<IEnumerable<HomeLoans>> SyncDatabaseAsync()
        {
            await SyncToCRMAsync();
            return _context.HomeLoans.Include(m => m.Residential);
        }

        // POST api/<HomeLoansController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HomeLoans homeLoan)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.HomeLoans.Add(homeLoan);

                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new { id = homeLoan.Id }, homeLoan);
            }
            catch (DbUpdateException DBException)
            {
                return BadRequest(DBException);
            }
        }

        // PUT: api/MedicalProfessionals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] HomeLoans homeLoan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != homeLoan.Id)
            {
                return BadRequest();
            }

            _context.Entry(homeLoan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeLoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<HomeLoansController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var homeLoan = await _context.HomeLoans.SingleOrDefaultAsync(m => m.Id == id);
            if (homeLoan == null)
            {
                return NotFound();
            }

            _context.HomeLoans.Remove(homeLoan);
            await _context.SaveChangesAsync();

            return Ok(homeLoan);
        }



        public async Task<List<HomeLoans>> GetHomeLoans()
        {
            await SyncToCRMAsync();
            var homeLoans = await _context.HomeLoans.ToListAsync();
            return homeLoans;
        }
        private async Task SyncToCRMAsync()
        {
            var homeLoans = await GetHomeLoansAsync(null, null, null, 100);
            foreach (var loan in homeLoans)
            {
                _context.HomeLoans.Add(loan);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<HomeLoans[]> GetHomeLoansAsync(int? loanAmount = 800000, int? lvr = 80, int? loanTerm = 30, int? totalOffers = 25)
        {
            string myJson = "{\"params\":{\"search\":{\"idLenders\":[\"6029ad56d2c6c0f7726d837bcec2c83253e29516d5923e232431215f3a380e3f\",\"19eb69f5cc9f2d4bec46c343935e2369\",\"6c88e1970c25d2cd3192b0e869ab80dd\",\"e233238467b090ac1e562ca95a0921f4\",\"a8de51bc04d85561c0c17d25a6d1317e\",\"1b913bda545f4e38635dbe20a65a94c2\",\"00739fe02e7084f84ef13fcdffa63561\",\"511aa909851d2437b24d4a180893ecaa\",\"5ae5b1fb1206b513b94b014cf0cd0019\",\"a5761cbcad92700e5a87a15c5174d070\",\"ef06f4da38cdac166b7fc455e5db2a25\",\"f67a8462fdcae616fc336851c4f58fa3\",\"5d990f0aca5d1714b045f98f8570bb5d\",\"13fd129dc4ba4e345cf3f45b2199bed2\",\"97ef085fa373d428e2591c77b9dfd585\",\"461f8dc5f35c92e57a0bf14be289d39a\",\"1006f206b1cd3e15216020cc9e11b806\",\"4509c066400b167d60660c638f1c4ea7\",\"49ccfd3517f32c5f167c0231133b5c2e\",\"1f5464dedc0980f170f4dc64e5a22cbf\",\"48e3d546cc6279dadae4ab3dace3ecd661cff2e1e6c2d79eddea12ff017503a6\",\"8813ab192fe4ad58d4c17d5615459f43\",\"1bbc7275e9359975173b06d547c6be98\",\"63da48b2ecb8a083e628e91f5ad0de4a\",\"5f105eb05600d02085b408e1ed0b4f1b\",\"08c0a6777b519c6cbdb73a8bc360d18f\",\"d0fe4a1fe6aa67cd0f8a777be6926e8f\"]," +
                "\"type\":\"RESIDENTIAL\",\"loanAmount\":" + loanAmount + ",\"lvr\":" + lvr + ",\"loanTerm\":" + loanTerm + ",\"rewards\":\"NOT_ESSENTIAL\",\"offsetNo\":0,\"limitNo\":" + totalOffers + "},\"comparisonRate\":[{\"amount\":800000,\"term\":30,\"termIo\":0,\"interestRate\":0,\"revertRate\":0," +
                "\"fees\":0,\"key\":\"search\"}],\"maximumBorrowing\":{\"key\":\"search\",\"lvr\":" + lvr + ",\"amount\":" + loanAmount + ",\"useLmi\":false,\"useGeneric\":false,\"products\":[],\"contacts\":[{\"key\":\"2b6a32b2-7018-40d5-a5d8-8bc450d98260\",\"keySpouse\":\"y\"," +
                "\"married\":false,\"dependents\":0,\"totalIncomeRental\":0,\"totalIncomeOtherTaxable\":0,\"totalIncomeNonTaxable\":0,\"totalIncomeBusinessLastTaxYear\":2020,\"totalIncomeBusinessPreviousTaxYear\":2020,\"totalInterestAndOtherAddBacksLastTaxYear\":0," +
                "\"totalInterestAndOtherAddBacksPreviousTaxYear\":0,\"totalProfitBeforeTaxLastTaxYear\":0,\"totalProfitBeforeTaxPreviousTaxYear\":0,\"totalDepreciationLastTaxYear\":0,\"totalDepreciationPreviousTaxYear\":0,\"liabilitiesCreditCards\":[]," +
                "\"liabilitiesMortgage\":[],\"liabilitiesPersonal\":[],\"liabilitiesVehicle\":[],\"liabilitiesOther\":[],\"hasEducationDebt\":false,\"isSelfEmployedInACompany\":false,\"housing\":{\"title\":\"Select one\",\"name\":\"none\",\"ordinal\":0}," +
                "\"purchaseOwnerOccupiedStatus\":true,\"postSettlementAddressStatus\":false,\"splitOwnership\":0,\"totalIncomeGrossSalary\":0,\"totalIncomeAllowance\":0,\"totalIncomeBonus\":0,\"totalIncomeCommissions\":0,\"totalIncomeOvertimeEssential\":0," +
                "\"totalIncomeOvertimeNonEssential\":0,\"totalIncomeRentalProperties\":0,\"totalIncomeBoarder\":0,\"liabilitiesOverdraft\":[],\"liabilitiesProposed\":[{\"key\":\"search\",\"balance\":800000,\"term\":360,\"termIo\":0,\"interestRate\":0,\"revertRate\":0," +
                "\"ownership\":100,\"isInvestment\":false}]}],\"expenses\":[{\"key\":\"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"idContacts\":[\"hhhhhhhh-hhhh-hhhh-hhhh-hhhhhhhhhhhh\"],\"foodAndSupermarket\":0,\"coffeesLunchesTakeaway\":0,\"entertainment\":0," +
                "\"domesticHolidays\":0,\"gymFeesSportOtherHealthAndWellness\":0,\"cigarettesAndAlcohol\":0,\"clothingShoesAndAccessories\":0,\"hairdressingAndGrooming\":0,\"phoneInternetAndPayTv\":0,\"mediaStreamingAndSubscriptionServices\":0,\"giftsAndCelebrations\":0," +
                "\"otherDiscretionaryExpenses\":0,\"recreationalVehicleRunningCosts\":0,\"pets\":0,\"publicEducationCosts\":0,\"privateEducationCosts\":0,\"tertiaryAndVocationalEducation\":0,\"childcare\":0,\"childOrSpousalMaintenance\":0,\"privateHealthInsurance\":0," +
                "\"doctorDentistPharmacyGlasses\":0,\"lifeTraumaIncomeInsurance\":0,\"essentialVehicleRunningCost\":0,\"publicTransportTaxisAndRideShareCommutingAirfares\":0,\"essentialVehicleInsurance\":0,\"currentRentExpense\":0,\"ongoingBoardExpense\":0," +
                "\"primaryResidenceRunningCosts\":0,\"primaryResidenceBodyCorp\":0,\"secondaryResidenceRunningCosts\":0,\"secondaryResidenceBodyCorp\":0,\"investmentPropertyRunningCosts\":0,\"investmentPropertyBodyCorp\":0}]}}}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQzN2UxYjM5LTljM2YtNDY5ZC04ZDE0LWVjMzk0MDdlZjdkNiIsImlkQ3VycmVudE9yZ2FuaXphdGlvbiI6ImIwMWQyNzAzLWM4NjAtNGFkNi05YjcyLTc0NzlhM2IyYjlmZSIsImlhdCI6MTYwNjM3ODQzNywiZXhwIjoxNjA3NTg4MDM3fQ.8gq6LdwRFEDWEXB4o51KoZh5KO5b4pg2Bh6CYehV8GY");

                var response = await client.PostAsync(
                    "https://vownet.salestrekker.com/api/v1/products/home-loan-available/",
                     new StringContent(myJson, Encoding.UTF8, "application/json"));
                var contentString = await response.Content.ReadAsStringAsync();

                var hoamLoans = HomeLoans.FromJson(contentString);

                return hoamLoans;
            }
        }

        //public async Task<HomeLoans[]> GetHomeLoansGraphQL()
        //{

        //}

        private bool HomeLoanExists(string id)
        {
            return _context.HomeLoans.Any(e => e.Id == id);
        }
    }
}
