using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public interface IProcess
    {
        void ImplementProcess();

        void ImplementNormalLocal();

        void ImplementDeletedLocal();

        void ImplementNoneLocal();
    }
}