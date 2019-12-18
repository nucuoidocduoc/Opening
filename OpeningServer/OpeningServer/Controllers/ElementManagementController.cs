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
    public class ElementManagementController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public ElementManagementController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllElementManagementAsync()
        {
            try {
                var elements = await _repository.ElementManagement.GetAllAsync();
                return Ok(elements);
            }
            catch (Exception) {
                throw;
            }
        }

        [HttpGet("[action]/{drawingName}")]
        public async Task<IActionResult> GetAllElementManagementExeptAsync(string drawingName)
        {
            try {
                var drawing = await _repository.Drawing.GetDrawingByNameAsync(drawingName);
                var elementsManage = await _repository.ElementManagement.GetAllExeptAsync(drawing.Id);
                return Ok(elementsManage.ConvertElementManagementCollection());
            }
            catch (Exception) {
                throw;
            }
        }

        //[HttpGet("[action]/{drawingName}")]
        //public async Task<IActionResult> GetAllElementManagementInDrawingAsync(string drawingName)
        //{
        //    try {
        //        var drawing = await _repository.Drawing.GetDrawingByNameAsync(drawingName);
        //        var elementsManage = await _repository.ElementManagement.GetAllInDrawingAsync(drawing.Id);
        //        return Ok(elementsManage);
        //    }
        //    catch (Exception) {
        //        throw;
        //    }
        //}
    }
}