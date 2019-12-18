using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.DTO
{
    public class ElementManagementSendDTO
    {
        public Guid Id { get; set; }
        public GeometryDTO Geometry { get; set; }
        public IEnumerable<string> DrawingsContain { get; set; }
    }
}