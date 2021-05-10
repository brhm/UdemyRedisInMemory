using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            List<string> nameList = new List<string>();

            if(db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x=> {
                    nameList.Add(x);
                });
            }

            return View(nameList);
        }

        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string name)
        {
            await db.ListRemoveAsync(listKey, name);

            return RedirectToAction("Index");
        }
        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey);
            return RedirectToAction("Index");
        }

    }
}
