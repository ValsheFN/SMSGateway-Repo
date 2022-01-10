using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSGateway.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public SchedulerController(ISchedulerService schedulerService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _schedulerService = schedulerService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> UpdateSmsStatus()
        {
            _recurringJobManager.AddOrUpdate("Update SMS Status",() => _schedulerService.UpdateSmsStatus(), "*/3 * * * *");
            return Ok("Update SMS Status job is running");
        }
    }
}
