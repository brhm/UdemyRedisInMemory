using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private  IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

        }
        public IActionResult Index()
        {

            // 1. yol değer zaman adında parametre var mı yokmu kontol ediyoruz
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            //2. yol

            if (!_memoryCache.TryGetValue("zaman",out string zamancache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }


            return View();
        }
        public IActionResult Show()
        {

            //cache silme işlemi.
            _memoryCache.Remove("zaman");

            //kontrol et varsa getir yoksa oluşturup değer set et
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                
                return DateTime.Now.ToString();
            });



            ViewBag.Zaman= _memoryCache.Get<string>("zaman");

            return View();
        }
    }
}
