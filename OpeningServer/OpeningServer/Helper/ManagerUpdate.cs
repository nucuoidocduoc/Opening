using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public class ManagerUpdate : IUpdatingData
    {
        private LocalDataModel _localDataModel;
        private IRepositoryWrapper _repository;
        private Guid _idDrawing;
        private IUpdatingData _create;
        private IUpdatingData _delete;
        private IUpdatingData _edit;
        private IDisconnect _disconnect;

        public ManagerUpdate(LocalDataModel localDataModel, IRepositoryWrapper repository)
        {
            _localDataModel = localDataModel;
            _repository = repository;
            _idDrawing = _repository.Drawing.FindByCondition(x => x.Name.Equals(_localDataModel.DrawingName)).FirstOrDefault().Id;
            _create = new CreatingUpdate(_localDataModel, _idDrawing, _repository);
            _edit = new EditingUpdate(_localDataModel, _idDrawing, _repository);
            _delete = new DeletingUpdate(_localDataModel, _idDrawing, _repository);
            _disconnect = new DisconnectUpdate(_localDataModel, _idDrawing, _repository);
        }

        public void ImplementUpdate()
        {
            _disconnect.ImplementDisconnect();
            _create.ImplementUpdate();
            _edit.ImplementUpdate();
            _delete.ImplementUpdate();
            RevisionUpdateAsync();
            _repository.SaveChangesAsync();
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