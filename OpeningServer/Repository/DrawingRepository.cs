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
    public class DrawingRepository : RepositoryBase<Drawing>, IDrawingRepository
    {
        public DrawingRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Drawing> GetDrawingByNameAsync(string name)
        {
            return await FindByCondition(draw => draw.Name.Equals(name)).Include(x => x.Elements).FirstOrDefaultAsync();
        }
    }
}