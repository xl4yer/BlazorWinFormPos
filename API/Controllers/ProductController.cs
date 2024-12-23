﻿using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
        public async Task<List<products>> Products()
        {
            var ret = await xservices.Products();
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