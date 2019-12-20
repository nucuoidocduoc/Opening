using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using OpeningServer.DTO;
using OpeningServer.Helper.Cluster;
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
        private IUpdatingData _localPullUpdate;
        private IUpdatingData _localPushUpdate;

        public ManagerUpdate(LocalDataModelDTO<ElementGetDTO> localDataModel, IRepositoryWrapper repository)
        {
            _localDataModel = localDataModel;
            _repository = repository;
            _idDrawing = _repository.Drawing.FindByCondition(x => x.Name.Equals(_localDataModel.DrawingName)).FirstOrDefault().Id;
        }

        public async Task<bool> ImplementUpdateAsync()
        {
            if (_localDataModel.OpeningsLocalPullAction != null && _localDataModel.OpeningsLocalPullAction.Count() > 0) {
                _localPullUpdate = new LocalPullUpdating(_localDataModel.OpeningsLocalPullAction, _idDrawing, _repository);
            }
            if (_localDataModel.OpeningsLocalPushAction != null && _localDataModel.OpeningsLocalPushAction.Count() > 0) {
                _localPushUpdate = new LocalPushUpdating(_localDataModel.OpeningsLocalPushAction, _idDrawing, _repository);
            }
            var tasks = new List<Task<bool>>();

            if (_localPushUpdate != null) {
                tasks.Add(_localPushUpdate.ImplementUpdateAsync());
            }
            if (_localPullUpdate != null) {
                tasks.Add(_localPullUpdate.ImplementUpdateAsync());
            }
            await Task.WhenAll(tasks);
            await UpdateProcessing.CreateRevisionAsync(_repository, _idDrawing);
            return true;
        }
    }
}