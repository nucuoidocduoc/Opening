using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public class DisconnectUpdate : Updating, IDisconnect
    {
        public DisconnectUpdate(LocalDataModelDTO<ElementGetDTO> localDataModel, Guid drawingId, IRepositoryWrapper repository)
            : base(localDataModel, drawingId, repository)
        {
        }

        public async void ImplementDisconnect()
        {
            foreach (var opening in _localDataModel.OpeningsDisconnect) {
                var element = await _repository.Element.FindByCondition(x => x.Id.Equals(opening.Id)).FirstOrDefaultAsync();
                _repository.Element.Delete(element);
            }
        }
    }
}