﻿using Contracts;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public abstract class BaseData : IClustingStatus<ElementGetDTO>
    {
        protected IEnumerable<ElementGetDTO> _data;
        protected Guid _idDrawing;
        protected IRepositoryWrapper _repository;

        public BaseData(IEnumerable<ElementGetDTO> data, IRepositoryWrapper repository, Guid drawingId)
        {
            _data = data;
            _idDrawing = drawingId;
            _repository = repository;
        }

        private IEnumerable<ElementGetDTO> _normalLocalSet;
        private IEnumerable<ElementGetDTO> _deletedLocalSet;
        private IEnumerable<ElementGetDTO> _nondeLocalSet;

        public IEnumerable<ElementGetDTO> NormalLocalSet
        {
            get
            {
                if (_normalLocalSet == null) {
                    _normalLocalSet = _data.Where(e => e.LocalStatus.Equals(Define.NORMAL));
                }
                return _normalLocalSet;
            }
        }

        public IEnumerable<ElementGetDTO> DeletedLocalSet
        {
            get
            {
                if (_deletedLocalSet == null) {
                    _deletedLocalSet = _data.Where(e => e.LocalStatus.Equals(Define.DELETED));
                }
                return _deletedLocalSet;
            }
        }

        public IEnumerable<ElementGetDTO> NoneLocalSet
        {
            get
            {
                if (_nondeLocalSet == null) {
                    _nondeLocalSet = _data.Where(e => e.LocalStatus.Equals(Define.NONE));
                }
                return _nondeLocalSet;
            }
        }
    }
}