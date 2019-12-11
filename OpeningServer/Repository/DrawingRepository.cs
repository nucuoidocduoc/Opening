using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class DrawingRepository : RepositoryBase<Drawing>, IDrawingRepository
    {
        public DrawingRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}