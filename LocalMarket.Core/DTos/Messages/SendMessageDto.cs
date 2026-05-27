

using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Messages
{
    public class SendMessageDto
    {
        [Required(ErrorMessage = "Content is required")]
        [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters")]
        public string Content { get; set; } = string.Empty;

        [RegularExpression("^(text|image)$",
            ErrorMessage = "Type must be 'text' or 'image'")]
        public string Type { get; set; } = "text";
    }
}
