using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class ElementManagementRepository : RepositoryBase<ElementManagement>, IElementManagementRepository
    {
        public ElementManagementRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}