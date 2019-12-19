using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.DTO
{
    public class LocalDataModelDTO<T>
    {
        public string DrawingName { get; set; }

        public IEnumerable<T> OpeningsLocalPullAction { get; set; }
        public IEnumerable<T> OpeningsLocalPushAction { get; set; }
    }
}