using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public interface IProcess
    {
        Task<bool> ImplementProcess();

        Task<bool> ImplementNormalLocal();

        Task<bool> ImplementDeletedLocal();

        Task<bool> ImplementNoneLocal();
    }
}