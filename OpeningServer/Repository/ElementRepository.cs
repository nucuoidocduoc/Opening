using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ElementRepository : RepositoryBase<Element>, IElementRepository
    {
        public ElementRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public Task<IEnumerable<Element>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Element>> GetAllElementsInDrawingAsync(Guid guid)
        {
            var elements = await RepositoryContext.Set<Element>()
                .Where(e => e.IdDrawing.Equals(guid) && !e.Status.Equals("Deleted"))
                .Include(e => e.ElementManagement)
                .ThenInclude(m => m.GeometryVersions).ToListAsync();
            return elements;
        }
    }
}