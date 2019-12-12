using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("Element")]
    public class Element
    {
        public Guid Id { get; set; }
        public Guid IdManager { get; set; }
        public Guid IdDrawing { get; set; }
        public Guid IdRevitElement { get; set; }
        public string Status { get; set; }

        [ForeignKey(nameof(IdManager))]
        public virtual ElementManagement ElementManagement { get; set; }

        [ForeignKey(nameof(IdDrawing))]
        public virtual Drawing Drawing { get; set; }
    }
}