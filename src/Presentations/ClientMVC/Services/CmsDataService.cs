using Caching;
using ClientMVC.Data;
using ClientMVC.Models;
using Models.Entities;
using Models.ResponseModels;
using System;

namespace ClientMVC.Services
{
    public class CmsDataService : ICmsDataService
    {
        private readonly ICacheProvider _cache;
        private readonly CmsConfig _cmsConfig;
        public CmsDataService(ICacheProvider cache, CmsConfig cmsConfig)
        {
            _cache = cache;
            _cmsConfig = cmsConfig;
        }
        public ContentDto GetByUrl(string page, int cacheTime = 5)
        {
            var cacheKey = $"MvcClient.GetByUrl.Url-{page}";
            string requestUrl = $"{_cmsConfig.ApiPath}/{_cmsConfig.ContentGetByUrl}?url={page}"; 

            var data = _cache.Get(cacheKey, TimeSpan.FromMinutes(cacheTime), () => CmsClient.Get<BaseResponse<ContentDto>>(requestUrl));
            return data.Data;
        }
    }
}
