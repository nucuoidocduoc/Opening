using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class CheckoutVersionRepository : RepositoryBase<CheckoutVersion>, ICheckoutVersionRepository
    {
        public CheckoutVersionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}