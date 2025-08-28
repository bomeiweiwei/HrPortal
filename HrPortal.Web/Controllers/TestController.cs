using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPortal.Services.Test;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HrPortal.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private ITestService _testService;
        public TestController(IWebHostEnvironment env, ITestService testService)
        {
            _env = env;
            _testService = testService;
        }
        [HttpGet]
        [Route("GetEnv")]
        public async Task<IActionResult> GetEnv()
        {
            var environment = _env.EnvironmentName;
            return Ok(new { environment });
        }
        [HttpGet]
        [Route("GetConnectResult")]
        public async Task<IActionResult> GetConnectResult()
        {
            var ok = await _testService.GetConnectResult();
            return Ok(new { ok });
        }
    }
}

