using System;
using System.Collections.Generic;
using System.Text;

namespace LocalMarket.Core.DTos.Reviews
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
