using Data.Mongo.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface INewsService
    {
        Content GetByUrl(string url, int cacheTime = 2);
        Task AddNews(Content model);
        List<Content> GetNews();
    }
}
