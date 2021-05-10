using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
           

            db.StringSet("name", "ibrahim keskin");
            db.StringSet("ziyaretci", 100);


            return View();
        }
        public IActionResult Show()
        {
            var name = db.StringGet("name");

            var name2 = db.StringGetRange("name", 0, 5);// return ibrah
            //değerini 1 arttır.
            db.StringIncrement("ziyaretci", 1);
            var ziyaretci = db.StringGet("ziyaretci");
            if (name.HasValue)
            {
                ViewBag.Name = name.ToString();
            }
            if (ziyaretci.HasValue)
            {
                ViewBag.ziyaretci = ziyaretci.ToString();
            }

            //değerini 5 azalt
            db.StringDecrement("ziyaretci", 5);
            var ziyaretci2 = db.StringGet("ziyaretci");


            return View();
        }
    }
}
