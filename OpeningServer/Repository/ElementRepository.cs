using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class ElementRepository : RepositoryBase<Element>, IElementRepository
    {
        public ElementRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}