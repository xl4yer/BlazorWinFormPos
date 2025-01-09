using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Services;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PurchaseController : Controller
    {

        PurchaseServices xservices;
      
        public PurchaseController(PurchaseServices xservices)
        {
            this.xservices = xservices;
        }

        [HttpGet]
        public async Task<List<purchase>> GetPurchase()
        {
            var ret = await xservices.GetPurchase();
            return ret;
        }

        [HttpGet]
        public async Task<double> GetTodaySales()
        {
            return await xservices.GetTodaySales();
        }

        [HttpGet]
        public async Task<double> GetMonthSales()
        {
            return await xservices.GetMonthSales();
        }

        [HttpGet]
        public async Task<ActionResult<List<purchase>>> GetMonthlySales()
        {
            var result = await xservices.GetMonthlySales();
            return Ok(result);
        }

    }
}
