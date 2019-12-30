using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("revision")]
    public class Revision
    {
        public Guid Id { get; set; }
        public Guid IdDrawing { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(IdDrawing))]
        public virtual Drawing Drawing { get; set; }

        public virtual IEnumerable<CheckoutVersion> CheckoutVersions { get; set; }
    }
}