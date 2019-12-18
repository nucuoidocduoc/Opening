using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.DTO
{
    public class ElementGetDTO : ElementSendDTO
    {
        public string Action { get; set; }
    }
}