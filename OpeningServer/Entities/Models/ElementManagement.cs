using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("ElementManagement")]
    public class ElementManagement
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}