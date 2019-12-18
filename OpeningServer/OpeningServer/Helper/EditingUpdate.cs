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
    public class EditingUpdate : Updating, IUpdatingData
    {
        public EditingUpdate(LocalDataModelDTO<ElementGetDTO> localDataModel, Guid drawingId, IRepositoryWrapper repository)
                : base(localDataModel, drawingId, repository)
        {
        }

        public async void ImplementUpdate()
        {
            foreach (var opening in _localDataModel.OpeningsEditedGeometryVersion) {
                var manager = await _repository.ElementManagement.FindByCondition(e => e.Id.Equals(opening.Id)).FirstOrDefaultAsync();
                GeometryVersion geometryVersion = new GeometryVersion() { Id = new Guid(), IdManager = manager.Id };
                _repository.GeometryVersion.Add(geometryVersion);
            }
        }
    }
}