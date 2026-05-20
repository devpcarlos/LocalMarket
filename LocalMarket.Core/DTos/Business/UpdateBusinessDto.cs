using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LocalMarket.Core.DTos.Business
{
    public class UpdateBusinessDto
    {
        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(20)]
        public string? Nit { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(80)]
        public string? City { get; set; }

        [Phone(ErrorMessage = "Invalid phone format")]
        public string? Phone { get; set; }

        public bool HasWhatsapp { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}
