using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IElementRepository : IRepositoryBase<Element>
    {
        Task<IEnumerable<Element>> GetAllAsync();

        Task<IEnumerable<Element>> GetAllElementsInDrawingAsync(Guid guid);
    }
}