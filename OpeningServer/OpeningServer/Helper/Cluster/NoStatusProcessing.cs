using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class NoStatusProcessing<T> : BaseData<T>, IProcess
    {
        public void ImplementProcess()
        {
            ImplementNormalLocal();
            ImplementNoneLocal();
        }

        public void ImplementNormalLocal()
        {
            if (NormalLocalSet == null || NormalLocalSet.Count() <= 0) {
                return;
            }
            if (typeof(T).Equals(typeof(LocalPushUpdating))) {
                foreach (var element in NormalLocalSet) {
                    UpdateProcessing.CreateElement(element, _repository, _idDrawing);
                }
            }
        }

        public void ImplementDeletedLocal()
        {
            throw new NotImplementedException();
        }

        public void ImplementNoneLocal()
        {
            if (NormalLocalSet == null || NormalLocalSet.Count() <= 0) {
                return;
            }
            if (typeof(T).Equals(typeof(LocalPushUpdating))) {
                foreach (var element in NormalLocalSet) {
                    UpdateProcessing.CreateElementFromStack(element, _repository, _idDrawing);
                }
            }
        }

        public NoStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}