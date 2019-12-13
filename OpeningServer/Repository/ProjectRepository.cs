using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Project> GetProjectByNameAsync()
        {
            return await FindByCondition(p => p.Name.Equals("VinHome")).Include(d => d.Drawings).FirstOrDefaultAsync();
        }
    }
}