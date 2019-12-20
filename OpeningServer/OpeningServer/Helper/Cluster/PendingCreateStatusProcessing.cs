using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class PendingCreateStatusProcessing : BaseData, IProcess
    {
        public Func<Type> TargetType { get; set; }

        public async Task<bool> ImplementProcess()
        {
            if (_data == null || _data.Count() <= 0) {
                return true;
            }
            var tasks = new List<Task<bool>>();
            if (TargetType.Invoke().Equals(typeof(LocalPushUpdating))) {
                foreach (var element in _data) {
                    tasks.Add(UpdateProcessing.UpdateWhenStatusOfElementChangeToDeletedAsync(element, _repository));
                }
            }
            else {
                foreach (var element in _data) {
                    tasks.Add(UpdateProcessing.ReGenerateElementBelowLocalAsync(element, _repository));
                }
            }
            await Task.WhenAll(tasks);
            return true;
        }

        public Task<bool> ImplementNormalLocal()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ImplementDeletedLocal()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ImplementNoneLocal()
        {
            throw new NotImplementedException();
        }

        public PendingCreateStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}