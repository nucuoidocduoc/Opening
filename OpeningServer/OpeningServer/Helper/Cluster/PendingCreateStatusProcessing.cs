using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class PendingCreateStatusProcessing<T> : BaseData<T>, IProcess
    {
        public void ImplementProcess()
        {
            if (_data == null || _data.Count() <= 0) {
                return;
            }
            if (typeof(T).Equals(typeof(LocalPushUpdating))) {
                foreach (var element in _data) {
                    UpdateProcessing.UpdateElementDeletedLocalToServerAsync(element, _repository);
                }
            }
            else {
                foreach (var element in _data) {
                    UpdateProcessing.UpdateElementStatusWithRecreateLocalAsync(element, _repository, Define.NORMAL);
                }
            }
        }

        public void ImplementNormalLocal()
        {
            throw new NotImplementedException();
        }

        public void ImplementDeletedLocal()
        {
            throw new NotImplementedException();
        }

        public void ImplementNoneLocal()
        {
            throw new NotImplementedException();
        }

        public PendingCreateStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}