using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class RouteViewModel
    {
        public int RouteID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public string RouteName { get; set; } = string.Empty;

        [Required]
        public string StartLocation { get; set; } = string.Empty;

        [Required]
        public string FinishLocation { get; set; } = string.Empty;

        public decimal Distance { get; set; }

        public string? RouteDescription { get; set; }
    }
}