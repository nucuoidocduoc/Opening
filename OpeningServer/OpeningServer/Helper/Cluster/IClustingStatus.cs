using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    internal interface IClustingStatus<T>
    {
        IEnumerable<T> NormalLocalSet { get; }
        IEnumerable<T> DeletedLocalSet { get; }
        IEnumerable<T> NoneLocalSet { get; }
    }
}