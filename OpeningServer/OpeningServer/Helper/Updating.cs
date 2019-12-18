using Contracts;
using Entities.Models;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public abstract class Updating
    {
        protected Guid _drawingId;
        protected IRepositoryWrapper _repository;
        protected LocalDataModelDTO<ElementGetDTO> _localDataModel;

        public Updating(LocalDataModelDTO<ElementGetDTO> localDataModel, Guid drawingId, IRepositoryWrapper repository)
        {
            _repository = repository;
            _drawingId = drawingId;
            _localDataModel = localDataModel;
        }
    }
}