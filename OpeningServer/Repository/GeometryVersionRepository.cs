using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class GeometryVersionRepository : RepositoryBase<GeometryVersion>, IGeometryVersionRepository
    {
        public GeometryVersionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}