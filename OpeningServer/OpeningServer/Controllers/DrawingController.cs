using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpeningServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawingController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public DrawingController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDrawingsAsync()
        {
            try {
                var drawings = await _repository.Drawing.GetAllDrawingAsync();
                if (drawings == null) {
                    return NotFound();
                }
                return Ok(drawings);
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}