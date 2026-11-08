using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public int AgeMinimum { get; set; }

        public int AgeMaximum { get; set; }

        public decimal Distance { get; set; }
    }
}