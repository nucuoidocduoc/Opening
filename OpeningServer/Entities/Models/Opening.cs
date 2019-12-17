using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Opening
    {
        public Guid Id { get; set; }
        public Guid IdManager { get; set; }
        public bool IsDisconnect { get; set; }
        public Guid IdRevitElement { get; set; }
        public GeometryVersion Geometry { get; set; }
        public string Action { get; set; }
    }
}