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
    public class LocalPushUpdating : Updating, IUpdatingData
    {
        public LocalPushUpdating(IEnumerable<ElementGetDTO> elements, Guid drawingId, IRepositoryWrapper repository)
           : base(elements, drawingId, repository)
        {
        }

        public async Task<bool> ImplementUpdateAsync()
        {
            InitData(typeof(LocalPushUpdating));
            var tasks = new List<Task<bool>>();
            if (_normalServer != null) {
                tasks.Add(_normalServer.ImplementProcess());
            }

            if (_pendingDeleteServer != null) {
                tasks.Add(_pendingDeleteServer.ImplementProcess());
            }

            if (_pendingCreateServer != null) {
                tasks.Add(_pendingCreateServer.ImplementProcess());
            }

            if (_noStatusServer != null) {
                _noStatusServer.ImplementProcess();
            }
            tasks.Add(ImplementDisconnectAsync());
            await Task.WhenAll(tasks);
            return true;
        }

        private async Task<bool> ImplementDisconnectAsync()
        {
            var disconnectElements = _elements.Where(x => x.LocalStatus.Equals(Define.DISCONNECT)).ToList();
            if (disconnectElements == null || disconnectElements.Count <= 0) {
                return true;
            }

            foreach (var ele in disconnectElements) {
                var eleFind = await _repository.Element.FindByCondition(x => x.Id.Equals(ele.Id)).FirstOrDefaultAsync();
                if (eleFind.Status.Equals(Define.PENDING_DELETE)) {
                    if (await UpdateProcessing.IsDeletedAllExept(eleFind.IdManager, eleFind.Id, _repository)) {
                        var manager = await _repository.ElementManagement.FindByCondition(x => x.Id.Equals(eleFind.IdManager)).FirstOrDefaultAsync();
                        manager.Status = Define.DELETED;
                        _repository.ElementManagement.Update(manager);
                    }
                }
                _repository.Element.Delete(eleFind);
            }
            return true;
        }
    }
}