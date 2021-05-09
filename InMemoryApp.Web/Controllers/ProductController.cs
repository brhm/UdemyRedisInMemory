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

            //2. yol
          
                MemoryCacheEntryOptions optionsAbsolute = new MemoryCacheEntryOptions();
            optionsAbsolute.AbsoluteExpiration = DateTime.Now.AddSeconds(10);//   AbsoluteExpiration cache'in kesin olarak bu süre sonunda sıfırlanmasını sağlar..
                //options.SetSlidingExpiration

                _memoryCache.Set<string>("ZamanAbsoluteExpiration", DateTime.Now.ToString(), optionsAbsolute);

            MemoryCacheEntryOptions optionsSlide = new MemoryCacheEntryOptions();
            optionsSlide.SlidingExpiration = TimeSpan.FromSeconds(10); //   SlidingExpiration her işlemde 10 sn ileri atar.
            _memoryCache.Set<string>("ZamanSlideExpiration", DateTime.Now.ToString(), optionsSlide);




            MemoryCacheEntryOptions optionsSlideAbsolude = new MemoryCacheEntryOptions();
            optionsSlideAbsolude.SlidingExpiration = TimeSpan.FromSeconds(10); //   SlidingExpiration her işlemde 10 sn ileri atar.
            optionsSlideAbsolude.AbsoluteExpiration = DateTime.Now.AddMinutes(1); //   SlidingExpiration her işlemde 10 sn ileri atar. AbsoluteExpiration  süresi dolunca tüm cache silinir. birlikte kullanılmaları daha efektif olur.

            optionsSlideAbsolude.Priority = CacheItemPriority.High;// cache dolarsa db de silinecek cachelerin önceliğini belirleyebilir. yada bazı cacheleri hiç sildirmeye biliriz.

            // buradaki delegeyi ayrı method olarakta yazıp çağıra bilirdik. yada  
            optionsSlideAbsolude.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set<string>("callback", $"cache sonlanma : {key} - {value} - {reason} -");
            });



            _memoryCache.Set<string>("ZamanSlideAbsoluteExpiration", DateTime.Now.ToString(), optionsSlideAbsolude);


            return View();
        }
        public IActionResult Show()
        {

            ////cache silme işlemi.
            //_memoryCache.Remove("zaman");

            ////kontrol et varsa getir yoksa oluşturup değer set et
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{

            //    return DateTime.Now.ToString();
            //});

            _memoryCache.TryGetValue<string>("ZamanAbsoluteExpiration", out string zamanCacheAbsolute);
            ViewBag.ZamanAbsoluteExpiration = zamanCacheAbsolute;


            _memoryCache.TryGetValue<string>("ZamanSlideExpiration", out string zamanCacheSlide);
            ViewBag.ZamanSlideExpiration = zamanCacheSlide;


            _memoryCache.TryGetValue<string>("ZamanSlideAbsoluteExpiration", out string zamanCacheSlideAbsolute);
            ViewBag.ZamanSlideAbsoluteExpiration = zamanCacheSlideAbsolute;
            //ViewBag.Zaman= _memoryCache.Get<string>("zaman");

            _memoryCache.TryGetValue<string>("callback", out string callback);
            ViewBag.callback = callback;

            return View();
        }
    }
}
