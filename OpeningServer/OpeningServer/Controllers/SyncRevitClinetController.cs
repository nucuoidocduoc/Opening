using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpeningServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncRevitClinetController : ControllerBase
    {
        public async Task<IActionResult> GetOpeningsOnDrawing(string drawingName)
        {
            try {
                return Ok();
            }
            catch (Exception) {
                throw;
            }
        }
    }
}