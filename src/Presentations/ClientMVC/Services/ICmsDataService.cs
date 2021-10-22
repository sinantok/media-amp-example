using Models.Entities;

namespace ClientMVC.Services
{
    public interface ICmsDataService
    {
        ContentDto GetByUrl(string url, int cacheTime = 5);
    }
}
