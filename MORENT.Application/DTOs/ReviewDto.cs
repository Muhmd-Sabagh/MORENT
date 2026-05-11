using System;
using System.Collections.Generic;
using System.Text;

namespace MORENT.Application.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string ReviewerJobTitle { get; set; } = string.Empty; // From Figma design
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
