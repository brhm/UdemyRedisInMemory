using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {

        private string setKey = "sozluk";      
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> nameList = new Dictionary<string, string>();

            if (db.KeyExists(setKey))
            {
                //db.SortedSetScan(setKey).ToList().ForEach(x =>
                //{
                //    //x.Element
                //    //x.Score
                //    nameList.Add(x.Element.ToString());
                //});

                db.HashGetAll(setKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.Name,x.Value);
                });

            }

            return View(nameList);
        }
        public IActionResult Add(string name, string value)
        {
            if (!db.KeyExists(setKey))
            {
                db.KeyExpire(setKey, DateTime.Now.AddMinutes(1));
            }
            db.HashSet(setKey, name,value);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string name)
        {
            await db.HashDeleteAsync(setKey, name);

            return RedirectToAction("Index");
        }
    }
}
