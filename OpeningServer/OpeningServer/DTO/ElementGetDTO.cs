using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.DTO
{
    public class ElementGetDTO : ElementSendDTO
    {
        public Action Action { get; set; }
        public Status LocalStatus { get; set; }
        public bool DifferenceGeometry { get; set; }
    }
}