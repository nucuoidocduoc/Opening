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
            await Task.WhenAll(tasks);
            return true;
        }
    }
}