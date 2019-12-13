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
    public class ProjectController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public ProjectController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProjectByName()
        {
            try {
                var project = await _repository.Project.GetProjectByNameAsync();
                foreach (var draw in project.Drawings) {
                    if (draw.Name.Equals("MEP")) {
                        draw.Category = "MEPpppp";
                    }
                }
                project.Drawings = null;
                _repository.SaveChangesAsync();
                project = await _repository.Project.GetProjectByNameAsync();
                return Ok(project);
            }
            catch (Exception ex) {
                return NotFound();
            }
        }
    }
}