using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class NoStatusProcessing : BaseData, IProcess
    {
        public Func<Type> TargetType { get; set; }

        public async Task<bool> ImplementProcess()
        {
            ImplementNormalLocal();
            ImplementNoneLocal();
            return true;
        }

        public async Task<bool> ImplementNormalLocal()
        {
            if (NormalLocalSet == null || NormalLocalSet.Count() <= 0) {
                return true;
            }
            if (TargetType.Invoke().Equals(typeof(LocalPushUpdating))) {
                foreach (var element in NormalLocalSet) {
                    UpdateProcessing.CreateElement(element, _repository, _idDrawing);
                }
            }
            return true;
        }

        public Task<bool> ImplementDeletedLocal()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ImplementNoneLocal()
        {
            if (NormalLocalSet == null || NormalLocalSet.Count() <= 0) {
                return true;
            }
            if (TargetType.Invoke().Equals(typeof(LocalPushUpdating))) {
                foreach (var element in NormalLocalSet) {
                    UpdateProcessing.CreateElementFromStack(element, _repository, _idDrawing);
                }
            }
            return true;
        }

        public NoStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}