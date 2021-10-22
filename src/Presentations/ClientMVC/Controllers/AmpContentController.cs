using ClientMVC.Helpers.Interfaces;
using ClientMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ClientMVC.Controllers
{
    public class AmpContentController : Controller
    {
        private readonly ILogger<AmpContentController> _logger;
        private readonly ICmsDataService _cmsDataService;
        private readonly IConfiguration _configuration;

        private readonly IAmpHelper _ampHelper;
        

        public AmpContentController(ILogger<AmpContentController> logger, ICmsDataService cmsDataService, IConfiguration configuration, IAmpHelper ampHelper)
        {
            _logger = logger;
            _cmsDataService = cmsDataService;
            _configuration = configuration;
            _ampHelper = ampHelper;
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

                //ampVa
                content.Text = _ampHelper.FixAmpValidationErrors(content.Text);

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
