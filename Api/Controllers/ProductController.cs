using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Services;

namespace Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        ProductServices xservices;

        public ProductController(ProductServices xservices)
        {
            this.xservices = xservices;
        }

       
        [HttpGet]
        //[Authorize(Policy = "AdminCashier")]
        public async Task<List<products>> GetProducts()
        {
            var ret = await xservices.GetProducts();
            return ret;
        }

        [HttpPost]
        public async Task<int> AddProduct([FromBody] products p)
        {
            var ret = await xservices.AddProduct(p);
            return ret;
        }

        [HttpPut]
        public async Task<int> UpdateProduct([FromBody] products p)
        {
            var ret = await xservices.UpdateProduct(p);
            return ret;
        }

        [HttpGet]
        public async Task<List<products>> SearchProduct(string s)
        {
            var ret = await xservices.SearchProduct(s);
            return ret;
        }
    }
}
