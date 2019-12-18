using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.DTO
{
    public class LocalDataModelDTO<T>
    {
        public string DrawingName { get; set; }
        public IEnumerable<T> NewOpeningsBelowLocal { get; set; }
        public IEnumerable<T> NewOpeningsFromStack { get; set; }
        public IEnumerable<T> OpeningsPendingDelete { get; set; }
        public IEnumerable<T> OpeningsDeletedBelowLocal { get; set; }
        public IEnumerable<T> OpeningsPendingCreate { get; set; }
        public IEnumerable<T> OpeningsEditedGeometryVersion { get; set; }
        public IEnumerable<T> OpeningsDisconnect { get; set; }
    }
}