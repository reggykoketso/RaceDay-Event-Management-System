
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        public int OrganiserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        [Range(0.01, 1000)]
        public decimal Distance { get; set; }

        [Required]
        public string EventType { get; set; } = "Run";

        // Azure event banner image URL
        public string? ImageUrl { get; set; }
    }
}
