using System;
using System.Collections.Generic;
using System.Text;

namespace LocalMarket.Core.Entities
{
    public class BusinessCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
