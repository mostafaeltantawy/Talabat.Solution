﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services
{
    public interface IResponseCacheService
    {
        // Cache Data 
        Task CacheResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime);
        Task<string?> GetCachedResponse(string CacheKey);

    }
}
