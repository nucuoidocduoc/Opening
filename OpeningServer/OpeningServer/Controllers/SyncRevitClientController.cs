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
    public class SyncRevitClientController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public SyncRevitClientController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SyncDataOfLocalAsync([FromBody]LocalDataModelDTO<ElementGetDTO> localDataModel)
        {
            IUpdatingData updatingFromLocal = new ManagerUpdate(localDataModel, _repository);
            using (var transaction = await _repository.StartTransaction()) {
                try {
                    await updatingFromLocal.ImplementUpdateAsync();
                    await _repository.SaveChangesAsync();
                    transaction.Commit();

                    return Ok();
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> TestPost([FromBody]LocalDataModelDTO<ElementGetDTO> localDataModel)
        {
            try {
                var model = localDataModel;
                return Ok();
            }
            catch (Exception ex) {
                string message = ex.StackTrace;
                throw;
            }
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> LocalRollbackAsync()
        //{
        //    try {
        //    }
        //    catch (Exception) {
        //        throw;
        //    }
        //}
    }
}