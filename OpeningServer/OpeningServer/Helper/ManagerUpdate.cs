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
    public class ManagerUpdate : IUpdatingData
    {
        private LocalDataModelDTO<ElementGetDTO> _localDataModel;
        private IRepositoryWrapper _repository;
        private Guid _idDrawing;

        public ManagerUpdate(LocalDataModelDTO<ElementGetDTO> localDataModel, IRepositoryWrapper repository)
        {
            _localDataModel = localDataModel;
            _repository = repository;
            _idDrawing = _repository.Drawing.FindByCondition(x => x.Name.Equals(_localDataModel.DrawingName)).FirstOrDefault().Id;
        }

        public void ImplementUpdate()
        {
        }

        private void RevisionUpdateAsync()
        {
            Revision revision = new Revision() { Id = new Guid(), IdDrawing = _idDrawing, CreatedDate = DateTime.Now };
            _repository.Revision.Add(revision);

            var elementsInDrawing = _repository.Element.FindByCondition(e => e.IdDrawing.Equals(_idDrawing) &&
            (e.Status.Equals("Normal") || e.Status.Equals("PendingCreate")))
            .Include(m => m.ElementManagement)
            .ThenInclude(x => x.GeometryVersions);

            foreach (var ele in elementsInDrawing) {
                CheckoutVersion checkoutVersion = new CheckoutVersion() {
                    Id = new Guid(),
                    IdGeometryVersion = ele.ElementManagement.GeometryVersions.FirstOrDefault().Id,
                    IdRevision = revision.Id
                };
                _repository.CheckoutVersion.Add(checkoutVersion);
            }
        }
    }
}