using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        private RepositoryContext _repositoryContext;
        private IProjectRepository _projectRepository;
        private IDrawingRepository _drawingRepository;
        private IElementRepository _elementRepository;
        private IElementManagementRepository _elementManagementRepository;
        private IGeometryVersionRepository _geometryVersionRepository;
        private ICheckoutVersionRepository _checkoutVersionRepository;
        private IRevisionRepository _revisionRepository;

        public IProjectRepository Project => _projectRepository ?? new ProjectRepository(_repositoryContext);

        public IDrawingRepository Drawing => _drawingRepository ?? new DrawingRepository(_repositoryContext);

        public IElementManagementRepository ElementManagement => _elementManagementRepository ?? new ElementManagementRepository(_repositoryContext);

        public IElementRepository Element => _elementRepository ?? new ElementRepository(_repositoryContext);

        public IGeometryVersionRepository GeometryVersion => _geometryVersionRepository ?? new GeometryVersionRepository(_repositoryContext);

        public ICheckoutVersionRepository CheckoutVersion => _checkoutVersionRepository ?? new CheckoutVersionRepository(_repositoryContext);

        public IRevisionRepository Revision => _revisionRepository ?? new RevisionRepository(_repositoryContext);

        public async Task SaveChangesAsync()
        {
            await _repositoryContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}