using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using MyCache.Models;
using Microsoft.Extensions.Primitives;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCache.Controllers
{
    public class HomeController : Controller
    {
        private IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var cts = new CancellationTokenSource();
            _memoryCache.Set("User", new UserContent
                { Username = "darren",
                Email ="darren@example.com",
                Roles =new List<string> { "Admin"}
            }, new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .AddExpirationToken(new CancellationChangeToken(cts.Token))
                .RegisterPostEvictionCallback((ecokey, value, reason, substate) => { }));
            return Json(true);
        }

        public IActionResult About()
        {
            UserContent user;
            _memoryCache.TryGetValue("User", out user);
            return Json(user);
        }
    }
}
