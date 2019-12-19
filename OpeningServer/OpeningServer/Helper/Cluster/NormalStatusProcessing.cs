using Contracts;
using Entities.Models;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class NormalStatusProcessing<T> : BaseData<T>, IProcess
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
                foreach (var element in NormalLocalSet) {
                    // Tao geometry
                    UpdateProcessing.UpdateElementGeometryLocalToServer(element, _repository);
                }
            }
        }

        public void ImplementDeletedLocal()
        {
            if (DeletedLocalSet == null || DeletedLocalSet.Count() <= 0) {
                return;
            }
            if (typeof(T).Equals(typeof(LocalPushUpdating))) {
                foreach (var elementGetDTO in DeletedLocalSet) {
                    UpdateProcessing.UpdateElementDeletedLocalToServerAsync(elementGetDTO, _repository);
                }
            }
            else {
                foreach (var elementGetDTO in DeletedLocalSet) {
                    UpdateProcessing.UpdateElementStatusWithRecreateLocalAsync(elementGetDTO, _repository, Define.NORMAL);
                }
            }
        }

        public void ImplementNoneLocal()
        {
            throw new NotImplementedException();
        }

        public NormalStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}