using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RevisionRepository : RepositoryBase<Revision>, IRevisionRepository
    {
        public RevisionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}