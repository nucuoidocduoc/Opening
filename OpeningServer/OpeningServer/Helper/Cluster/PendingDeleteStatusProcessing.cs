using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class PendingDeleteStatusProcessing : BaseData, IProcess
    {
        public Func<Type> TargetType { get; set; }

        public async Task<bool> ImplementProcess()
        {
            var tasks = new List<Task<bool>>();
            tasks.Add(ImplementNormalLocal());
            tasks.Add(ImplementDeletedLocal());
            await Task.WhenAll(tasks);
            return true;
        }

        public async Task<bool> ImplementNormalLocal()
        {
            if (NormalLocalSet == null || NormalLocalSet.Count() <= 0) {
                return true;
            }
            var tasks = new List<Task<bool>>();
            if (TargetType.Invoke().Equals(typeof(LocalPushUpdating))) {
                foreach (var element in DeletedLocalSet) {
                    tasks.Add(UpdateProcessing.StatusChangeFromPendingDeleteToNormalAsync(element, _repository));
                }
            }
            else {
                foreach (var element in DeletedLocalSet) {
                    tasks.Add(UpdateProcessing.StatusChangeFromPendingDeleteToDeletedAsync(element, _repository));
                }
            }
            await Task.WhenAll(tasks);
            return true;
        }

        public async Task<bool> ImplementDeletedLocal()
        {
            if (DeletedLocalSet == null || DeletedLocalSet.Count() <= 0) {
                return true;
            }
            var tasks = new List<Task<bool>>();
            foreach (var element in DeletedLocalSet) {
                tasks.Add(UpdateProcessing.StatusChangeFromPendingDeleteToDeletedAsync(element, _repository));
            }
            await Task.WhenAll(tasks);
            return true;
        }

        public Task<bool> ImplementNoneLocal()
        {
            throw new NotImplementedException();
        }

        public PendingDeleteStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}