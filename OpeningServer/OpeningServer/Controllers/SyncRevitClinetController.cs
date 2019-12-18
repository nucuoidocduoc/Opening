using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpeningServer.DTO;
using OpeningServer.Helper;

namespace OpeningServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncRevitClinetController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public SyncRevitClinetController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SyncDataOfLocalAsync([FromBody]LocalDataModelDTO<ElementGetDTO> localDataModel)
        {
            try {
                IUpdatingData updatingFromLocal = new ManagerUpdate(localDataModel, _repository);
                updatingFromLocal.ImplementUpdate();
                await _repository.SaveChangesAsync();
                return Ok();
            }
            catch (Exception) {
                throw;
            }
        }
    }
}