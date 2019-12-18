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
    public class ElementManagementRepository : RepositoryBase<ElementManagement>, IElementManagementRepository
    {
        public ElementManagementRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<ElementManagement>> GetAllAsync()
        {
            var eleManas = await RepositoryContext.Set<ElementManagement>().Include(x => x.GeometryVersions).Include(x => x.Elements).ToListAsync();
            return eleManas;
        }

        public async Task<IEnumerable<ElementManagement>> GetAllExeptAsync(Guid drawingExcept)
        {
            var eleManas = await RepositoryContext.Set<ElementManagement>()
                .Include(x => x.GeometryVersions)
                .Include(x => x.Elements)
                .ThenInclude(e => e.Drawing)
                .Where(x => x.Status.Equals("Normal") && !x.Elements.Any(e => e.IdDrawing.Equals(drawingExcept))).ToListAsync();
            return eleManas;
        }

        public async Task<IEnumerable<ElementManagement>> GetAllInDrawingAsync(Guid drawing)
        {
            var eleManas = await RepositoryContext.Set<ElementManagement>()
                .Include(x => x.GeometryVersions)
                .Include(x => x.Elements)
                .Where(x => x.Elements.Any(e => e.IdDrawing.Equals(drawing))).ToListAsync();
            return eleManas;
        }
    }
}