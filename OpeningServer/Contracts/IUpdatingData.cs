using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUpdatingData
    {
        Task<bool> ImplementUpdateAsync();
    }
}