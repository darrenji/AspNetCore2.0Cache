using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using MyCache.Models;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCache.Controllers
{
    public class HomeController : Controller
    {
        private IDistributedCache _distributedCache;

        public HomeController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            _distributedCache.SetString("User", JsonConvert.SerializeObject(new UserContent {
                Username = "DARREN",
                Email = "darren@example.com",
                Roles = new List<string> { "admin"}
            }), new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            return Json(true);
        }

        public IActionResult About()
        {
            UserContent user = JsonConvert.DeserializeObject<UserContent>(_distributedCache.GetString("User"));
            return Json(user);
        }
    }
}
