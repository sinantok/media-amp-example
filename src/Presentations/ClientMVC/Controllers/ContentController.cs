using ClientMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ClientMVC.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly ICmsDataService _cmsDataService;
        private readonly IConfiguration _configuration;
        public ContentController(ILogger<ContentController> logger, ICmsDataService cmsDataService, IConfiguration configuration)
        {
            _logger = logger;
            _cmsDataService = cmsDataService;
            _configuration = configuration;
        }

        public IActionResult GetView(string page)
        {
            try
            {
                if (string.IsNullOrEmpty(page))
                    RedirectToAction("Index", "Home");

                int cacheTime = _configuration.GetValue<int>("Caching:ContentCacheTime");
                var content = _cmsDataService.GetByUrl(page, cacheTime);
                if (content == null)
                    return NotFound();

                return View(content.TemplateTarget, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
