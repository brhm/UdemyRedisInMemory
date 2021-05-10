using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public  async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name", "ibrahim", options);
           await  _distributedCache.SetStringAsync("surname", "keskin", options);

            Product product = new Product() { Id = 1, Name = "Kalem", Price = 15.9M, Stock = 10 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            await _distributedCache.SetStringAsync("product:1", jsonProduct,options);

            Product product2 = new Product() { Id = 2, Name = "Kalem2", Price = 19.9M, Stock = 10 };

            string jsonProduct2 = JsonConvert.SerializeObject(product2);

            Byte[] productByte = Encoding.UTF8.GetBytes(jsonProduct2);
            await _distributedCache.SetAsync("product:2", productByte, options);


            return View();
        }

        public async Task<IActionResult> Show()
        {
            var name=_distributedCache.GetString("name");
            var surname=await _distributedCache.GetStringAsync("surname");

            ViewBag.Name = name;
            ViewBag.Surname = surname;



            var productJson=await _distributedCache.GetStringAsync("product:1");
            Product product = JsonConvert.DeserializeObject<Product>(productJson);
            ViewBag.Product = product;


            var productByte =  await _distributedCache.GetAsync("product:2");

            string productJson2 = Encoding.UTF8.GetString(productByte);
            Product product2 = JsonConvert.DeserializeObject<Product>(productJson2);
            ViewBag.Product2 = product2;

            return View();
        }
        public IActionResult Remove()
        {

             _distributedCache.Remove("name");

            return View();
        }
    }
}
