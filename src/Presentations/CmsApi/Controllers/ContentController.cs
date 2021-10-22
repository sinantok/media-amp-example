using AutoMapper;
using Data.Mongo.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.ResponseModels;
using Services.Interface;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace CmsApi.Controllers
{
    [EnableCors("myclients")]
    [Route("api/v1/[controller]/[Action]")]
    [ApiController]
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public ContentController(ILogger<ContentController> logger, INewsService newsService, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _newsService = newsService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetByUrl(string url)
        {
            try
            {
                int cacheTime = _configuration.GetValue<int>("Caching:ContentCacheTime");
                var content = _newsService.GetByUrl(url, 2);
                var result = _mapper.Map<Content, ContentDto>(content);

                return Ok(new BaseResponse<object>(result, "CmsApi GetByUrl Result"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException($"{ex.Message}") { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
