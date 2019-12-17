using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class LocalDataModel
    {
        public string DrawingName { get; set; }
        public IEnumerable<Opening> NewOpeningsBelowLocal { get; set; }
        public IEnumerable<Opening> NewOpeningsFromStack { get; set; }
        public IEnumerable<Opening> OpeningsDisconnect { get; set; }
        public IEnumerable<Opening> OpeningsPendingDelete { get; set; }
        public IEnumerable<Opening> OpeningsDeletedBelowLocal { get; set; }
        public IEnumerable<Opening> OpeningFromPendingCreate { get; set; }
        public IEnumerable<Opening> OpeningEditedGeometryVersion { get; set; }
    }
}