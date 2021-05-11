using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string setKey = "sortednames";
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(setKey))
            {
                //db.SortedSetScan(setKey).ToList().ForEach(x =>
                //{
                //    //x.Element
                //    //x.Score
                //    nameList.Add(x.Element.ToString());
                //});

                db.SortedSetRangeByRank(setKey,order:Order.Descending).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });

            }

            return View(nameList);
        }
        public IActionResult Add(string name,int score)
        {
            if (!db.KeyExists(setKey))
            {
                db.KeyExpire(setKey, DateTime.Now.AddMinutes(1));
            }
            db.SortedSetAdd(setKey, name, score);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string name)
        {
            await db.SortedSetRemoveAsync(setKey, name);

            return RedirectToAction("Index");
        }
    }
}
