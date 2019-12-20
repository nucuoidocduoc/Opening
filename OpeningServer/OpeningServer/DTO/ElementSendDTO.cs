using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.DTO
{
    public class ElementSendDTO
    {
        public Guid Id { get; set; }
        public Guid IdManager { get; set; }
        public Guid IdRevitElement { get; set; }
        public string ServerStatus { get; set; }
        public GeometryDTO Geometry { get; set; }
    }
}