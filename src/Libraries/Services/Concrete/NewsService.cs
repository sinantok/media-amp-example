using Caching;
using Data.Mongo.Collections;
using Data.Mongo.Repo;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Concrete
{
    public class NewsService : INewsService
    {
        private readonly IMongoRepository<Content> _newsRepository;
        private readonly ICacheProvider _cache;
        public NewsService(IMongoRepository<Content> newsRepository, ICacheProvider cache)
        {
            _newsRepository = newsRepository;
            _cache = cache;
        }
        public async Task AddNews(Content model)
        {
            await _newsRepository.AddAsync(model);
        }

        public Content GetByUrl(string url, int cacheTime = 2)
        {
            string cacheKey = $"Api.GetByUrl.Url-{url}";
            return _cache.Get(cacheKey, TimeSpan.FromMinutes(cacheTime), () => _newsRepository.Find(x => x.Url.Equals(url)).FirstOrDefault());
        }

        public List<Content> GetNews()
        {
            return _newsRepository.GetAll();
        }

    }
}