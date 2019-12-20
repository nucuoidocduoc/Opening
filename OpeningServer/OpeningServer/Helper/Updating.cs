using Contracts;
using Entities.Models;
using OpeningServer.DTO;
using OpeningServer.Helper.Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public abstract class Updating
    {
        protected Guid _drawingId;
        protected IRepositoryWrapper _repository;
        protected IEnumerable<ElementGetDTO> _elements;

        protected NormalStatusProcessing _normalServer;
        protected PendingDeleteStatusProcessing _pendingDeleteServer;
        protected PendingCreateStatusProcessing _pendingCreateServer;
        protected NoStatusProcessing _noStatusServer;

        public Updating(IEnumerable<ElementGetDTO> elements, Guid drawingId, IRepositoryWrapper repository)
        {
            _repository = repository;
            _drawingId = drawingId;
            _elements = elements;
        }

        protected void InitData(Type type)
        {
            if (_elements.Count() > 0) {
                var elementsNormal = _elements.Where(x => x.ServerStatus.Equals(Define.NORMAL));
                if (elementsNormal != null && elementsNormal.Count() > 0) {
                    _normalServer = new NormalStatusProcessing(elementsNormal, _repository, _drawingId);
                    _normalServer.TargetType = () => type;
                }

                var elementsPendingDelete = _elements.Where(x => x.ServerStatus.Equals(Define.PENDING_DELETE));
                if (elementsPendingDelete != null && elementsPendingDelete.Count() > 0) {
                    _pendingDeleteServer = new PendingDeleteStatusProcessing(elementsPendingDelete, _repository, _drawingId);
                    _pendingDeleteServer.TargetType = () => type;
                }

                var elementsPendingCreate = _elements.Where(x => x.ServerStatus.Equals(Define.PENDING_CREATE));
                if (elementsPendingCreate != null && elementsPendingCreate.Count() > 0) {
                    _pendingCreateServer = new PendingCreateStatusProcessing(elementsPendingCreate, _repository, _drawingId);
                    _pendingCreateServer.TargetType = () => type;
                }

                var elementsNoStatus = _elements.Where(x => x.ServerStatus.Equals(string.Empty));
                if (elementsNoStatus != null && elementsNoStatus.Count() > 0) {
                    _noStatusServer = new NoStatusProcessing(elementsNoStatus, _repository, _drawingId);
                    _noStatusServer.TargetType = () => type;
                }
            }
        }
    }
}