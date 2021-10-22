using Core.Helpers;
using Data.Mongo.Collections;
using Newtonsoft.Json;
using Services.Interface;
using System.IO;
using System.Linq;

namespace CmsApi.Seeds
{
    public static class CheckNews
    {
        public static void SeedNews(INewsService newsService)
        {
            var newsList = newsService.GetNews();
            if (newsList == null || !newsList.Any())
            {
                var newsJson = new Content();
                using (StreamReader r = new StreamReader(@"data.json"))
                {
                    string json = r.ReadToEnd();
                    newsJson = JsonConvert.DeserializeObject<Content>(json);
                }
                if (newsJson != null)
                {
                    string slugUrl = UrlHelper.GetSlugUrl(newsJson.Title, newsJson.NewsId);
                    newsJson.Url = slugUrl;
                    newsJson.TemplateTarget = !string.IsNullOrEmpty(newsJson.ContentType) && newsJson.ContentType.Equals("Article") ? "NewsDetailTemplate" : "";
                    newsService.AddNews(newsJson);
                }
            }
        }
    }
}
