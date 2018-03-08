在Startup.cs中设置：

```
public class Startup
{
	public void ConfigureService(IServiceCollection services)
	{
		services.AddMemoryCache();
		serivces.AddMvc();
	}
}
```

在控制器中注入：

```
public class HomeController : Controller
{
	private IMemoryCache _memoryCache;

	public HomeController(IMemoryCache memoryCache)
	{
		_meoryCache = memoryCache;
	}

	public IActionResult Index()
	{
		var cts = new CancellationTokenSource();
		_meoryCache.Set("User", new UserContent{
			Username="",
			Email="",
			Roles = new List<string>{"Admin"}
		}, new MemoryCacheEntryOptions()
			.SetPriority(CacheItemPriority.High)
			.SetSlidingExpiration(TimeSpan.FromMinutes(10))
			.setAbsoluteExpiration(TimeSpan.FromMinutes(10))
			.AddExpirationToken(new CancellationChangeToken(cts.Token))
			.RegisterPostEvicionCallback((echokey, value, reason, substate) => {});
		);
		return Json(true);
	}

	public IActionResult About(){
		UserContent user;
		_momoryCache.TryGetValue("User", out user);
		return Josn(user);
	}
}
```

> 设置或获取时候可以使泛型类型

```
cache.Set<string>("mykey");

cache.Get<string>("mykey");
```

> 在设置之前判断键是否存在

```
if(!chace.TryGetValue<string>("mykey", out string  timestamp))
{
	cache.Set<string>("timestamp", DateTime.Now.ToString());
}
```

> 如果有就获取，没有就创建

```
string timestamp = cahce.GetOrCreate<string>("timestamp", entry => {
	return DateTime.Now.ToString();
});
```