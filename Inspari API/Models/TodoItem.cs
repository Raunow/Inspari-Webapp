using Inspari_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspari_API.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsComplete { get; set; }
        public DateTime Creation { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
