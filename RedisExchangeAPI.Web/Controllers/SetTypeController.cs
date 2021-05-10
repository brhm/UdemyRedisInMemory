using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string setKey = "names";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();

            if(db.KeyExists(setKey))
            {
                db.SetMembers(setKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }

            return View(nameList);
        }
        public IActionResult Add(string name)
        {
            if (!db.KeyExists(setKey))
            {
                db.KeyExpire(setKey, DateTime.Now.AddMinutes(1));
            }
            db.SetAdd(setKey, name);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string name)
        {
            await db.SetRemoveAsync(setKey, name);

            return RedirectToAction("Index");
        }
    }
}
