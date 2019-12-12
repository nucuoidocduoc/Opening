using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("CheckoutVersion")]
    public class CheckoutVersion
    {
        public Guid Id { get; set; }

        public Guid IdRevision { get; set; }

        public Guid IdGeometryVersion { get; set; }

        [ForeignKey(nameof(IdRevision))]
        public virtual Revision Revision { get; set; }

        [ForeignKey(nameof(IdGeometryVersion))]
        public virtual GeometryVersion GeometryVersion { get; set; }
    }
}