using Contracts;
using Entities.Models;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public class NormalStatusProcessing : BaseData, IProcess
    {
        public Func<Type> TargetType { get; set; }

        public async Task<bool> ImplementProcess()
        {
            ImplementNormalLocal();
            await ImplementDeletedLocal();
            return true;
        }

        public async Task<bool> ImplementNormalLocal()
        {
            if (NormalLocalSet == null || NormalLocalSet.Count() <= 0) {
                return true;
            }
            if (TargetType.Invoke().Equals(typeof(LocalPushUpdating))) {
                foreach (var element in NormalLocalSet) {
                    // Tao geometry
                    UpdateProcessing.CreateNewGeometryVersion(element, _repository);
                }
            }
            return true;
        }

        public async Task<bool> ImplementDeletedLocal()
        {
            if (DeletedLocalSet == null || DeletedLocalSet.Count() <= 0) {
                return true;
            }
            var tasks = new List<Task<bool>>();
            if (TargetType.Invoke().Equals(typeof(LocalPushUpdating))) {
                foreach (var elementGetDTO in DeletedLocalSet) {
                    tasks.Add(UpdateProcessing.UpdateWhenStatusOfElementChangeToDeletedAsync(elementGetDTO, _repository));
                }
            }
            else {
                foreach (var elementGetDTO in DeletedLocalSet) {
                    tasks.Add(UpdateProcessing.ReGenerateElementBelowLocalAsync(elementGetDTO, _repository));
                }
            }
            await Task.WhenAll(tasks);
            return true;
        }

        public Task<bool> ImplementNoneLocal()
        {
            throw new NotImplementedException();
        }

        public NormalStatusProcessing(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId) : base(data, repository, drawingId)
        {
        }
    }
}