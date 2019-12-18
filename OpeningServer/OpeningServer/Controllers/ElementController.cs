using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpeningServer.Helper;

namespace OpeningServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElementController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public ElementController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet("[action]/{drawingName}")]
        public async Task<IActionResult> GetAllElementInDrawing(string drawingName)
        {
            try {
                var drawing = await _repository.Drawing.GetDrawingByNameAsync(drawingName);
                var elements = await _repository.Element.GetAllElementsInDrawingAsync(drawing.Id);
                return Ok(elements.ConvertElementCollection());
            }
            catch (Exception) {
                throw;
            }
        }
    }
}