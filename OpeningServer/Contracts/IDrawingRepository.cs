using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDrawingRepository : IRepositoryBase<Drawing>
    {
        Task<Drawing> GetDrawingByNameAsync(string name);

        Task<IEnumerable<Drawing>> GetAllDrawingAsync();
    }
}