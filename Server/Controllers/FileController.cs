using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SMSGateway.Server.Models;

namespace SMSGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        // /api/file/importcontact
        [HttpPost("ImportContact")]
        public async Task<IActionResult> ImportContact(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                
            }

            return Ok();
        }
    }
}
