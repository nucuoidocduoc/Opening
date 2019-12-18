using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IElementManagementRepository : IRepositoryBase<ElementManagement>
    {
        Task<IEnumerable<ElementManagement>> GetAllAsync();

        Task<IEnumerable<ElementManagement>> GetAllExeptAsync(Guid drawingExcept);

        Task<IEnumerable<ElementManagement>> GetAllInDrawingAsync(Guid drawing);
    }
}