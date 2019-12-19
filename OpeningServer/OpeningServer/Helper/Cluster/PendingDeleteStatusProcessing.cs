using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class PendingDeleteStatusProcessing<T> : BaseData<T>, IProcess
    {
        public void ImplementProcess()
        {
            ImplementNormalLocal();
            ImplementDeletedLocal();
        }

        public void ImplementNormalLocal()
        {
            if (NormalLocalSet == null || NormalLocalSet.Count() <= 0) {
                return;
            }
            if (typeof(T).Equals(typeof(LocalPushUpdating))) {
                foreach (var element in DeletedLocalSet) {
                    UpdateProcessing.UpdateElementRecreateOnServerAsync(element, _repository);
                }
            }
            else {
                foreach (var element in DeletedLocalSet) {
                    UpdateProcessing.UpdateElementStatusWithDeletedLocalAndServerWithAsync(element, _repository, Define.DELETED);
                }
            }
        }

        public void ImplementDeletedLocal()
        {
            if (DeletedLocalSet == null || DeletedLocalSet.Count() <= 0) {
                return;
            }

            foreach (var element in DeletedLocalSet) {
                UpdateProcessing.UpdateElementStatusWithDeletedLocalAndServerWithAsync(element, _repository, Define.DELETED);
            }
        }

        public void ImplementNoneLocal()
        {
            throw new NotImplementedException();
        }

        public PendingDeleteStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}