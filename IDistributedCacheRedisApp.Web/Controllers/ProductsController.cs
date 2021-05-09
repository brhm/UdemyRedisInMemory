using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
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

            return View();
        }

        public async Task<IActionResult> Show()
        {
            var name=_distributedCache.GetString("name");
            var surname=await _distributedCache.GetStringAsync("surname");
            ViewBag.Name = name;
            ViewBag.Surname = surname;

            return View();
        }
        public IActionResult Remove()
        {

             _distributedCache.Remove("name");

            return View();
        }
    }
}
