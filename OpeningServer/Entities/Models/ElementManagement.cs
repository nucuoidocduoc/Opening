using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("elementmanagement")]
    public class ElementManagement
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public virtual IEnumerable<Element> Elements { get; set; }

        public virtual IEnumerable<GeometryVersion> GeometryVersions { get; set; }
    }
}