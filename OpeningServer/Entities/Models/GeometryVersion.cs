using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("geometryversion")]
    public class GeometryVersion
    {
        public Guid Id { get; set; }
        public Guid IdManager { get; set; }
        public string Geometry { get; set; }
        public string Direction { get; set; }
        public string Original { get; set; }
        public string Version { get; set; }
        public DateTime CreatedDate { get; set; }

        //public Guid IdUserEdited { get; set; }
        public string Status { get; set; }

        [ForeignKey(nameof(IdManager))]
        public virtual ElementManagement ElementManagement { get; set; }
    }
}