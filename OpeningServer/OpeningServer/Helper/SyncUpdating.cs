using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public class SyncUpdating : IUpdatingData
    {
        private LocalDataModelDTO<ElementGetDTO> _data;
        private IRepositoryWrapper _repository;

        public SyncUpdating(LocalDataModelDTO<ElementGetDTO> data, IRepositoryWrapper repository)
        {
            _data = data;
            _repository = repository;
        }

        public void ImplementUpdate()
        {
            throw new NotImplementedException();
        }
    }
}