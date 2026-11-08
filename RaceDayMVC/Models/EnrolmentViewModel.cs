using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }

        public int ParticipantID { get; set; }

        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public string EventName { get; set; } = "";

        public string CategoryName { get; set; } = "";
    }

    public class EnrolmentCreateViewModel
    {
        [Required]
        public int EventID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public List<EventViewModel> Events { get; set; } = new();

        public List<CategoryViewModel> Categories { get; set; } = new();
    }
}