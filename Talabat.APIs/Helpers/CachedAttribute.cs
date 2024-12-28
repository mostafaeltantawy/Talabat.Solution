using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSecond;
        public CachedAttribute(int ExpireTimeInSecond)
        {
            _expireTimeInSecond = ExpireTimeInSecond;

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var CacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var CachedResponse = await CacheService.GetCachedResponse(CacheKey);
            if (!string.IsNullOrEmpty(CachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;
            }
          var ExecutedEndPointContext =   await next.Invoke(); //will execute the endpoint

            if (ExecutedEndPointContext.Result is OkObjectResult result) 
            {
                await CacheService.CacheResponseAsync(CacheKey, result , TimeSpan.FromSeconds(_expireTimeInSecond));
            }

        }




        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
