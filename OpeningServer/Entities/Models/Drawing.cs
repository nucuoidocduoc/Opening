using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("Drawing")]
    public class Drawing
    {
        public Guid Id { get; set; }
        public Guid IdProject { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}