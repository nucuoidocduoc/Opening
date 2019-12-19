using Contracts;
using OpeningServer.DTO;
using OpeningServer.Helper.Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public class LocalPushUpdating : Updating, IUpdatingData
    {
        private NormalStatusProcessing<LocalPushUpdating> _normalServer;
        private PendingDeleteStatusProcessing<LocalPushUpdating> _pendingDeleteServer;
        private PendingCreateStatusProcessing<LocalPushUpdating> _pendingCreateServer;
        private NoStatusProcessing<LocalPushUpdating> _noStatusServer;

        public LocalPushUpdating(LocalDataModelDTO<ElementGetDTO> localDataModel, Guid drawingId, IRepositoryWrapper repository)
           : base(localDataModel, drawingId, repository)
        {
            if (_localDataModel.OpeningsLocalPushAction.Count() > 0) {
                var elementsNormal = _localDataModel.OpeningsLocalPushAction.Where(x => x.ServerStatus.Equals(Define.NORMAL));
                if (elementsNormal != null && elementsNormal.Count() > 0) {
                    _normalServer = new NormalStatusProcessing<LocalPushUpdating>(elementsNormal, _repository, _drawingId);
                }

                var elementsPendingDelete = _localDataModel.OpeningsLocalPushAction.Where(x => x.ServerStatus.Equals(Define.PENDING_DELETE));
                if (elementsPendingDelete != null && elementsPendingDelete.Count() > 0) {
                    _pendingDeleteServer = new PendingDeleteStatusProcessing<LocalPushUpdating>(elementsPendingDelete, _repository, _drawingId);
                }

                var elementsPendingCreate = _localDataModel.OpeningsLocalPushAction.Where(x => x.ServerStatus.Equals(Define.PENDING_CREATE));
                if (elementsPendingCreate != null && elementsPendingCreate.Count() > 0) {
                    _pendingCreateServer = new PendingCreateStatusProcessing<LocalPushUpdating>(elementsPendingCreate, _repository, _drawingId);
                }

                var elementsNoStatus = _localDataModel.OpeningsLocalPushAction.Where(x => x.ServerStatus.Equals(string.Empty));
                if (elementsNoStatus != null && elementsNoStatus.Count() > 0) {
                    _noStatusServer = new NoStatusProcessing<LocalPushUpdating>(elementsNoStatus, _repository, _drawingId);
                }
            }
        }

        public void ImplementUpdate()
        {
            if (_normalServer != null) {
                _normalServer.ImplementProcess();
            }

            if (_pendingDeleteServer != null) {
                _pendingDeleteServer.ImplementProcess();
            }

            if (_pendingCreateServer != null) {
                _pendingCreateServer.ImplementProcess();
            }

            if (_noStatusServer != null) {
                _noStatusServer.ImplementProcess();
            }
        }
    }
}