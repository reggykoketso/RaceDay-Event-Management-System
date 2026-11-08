
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class ResultViewModel
    {
        public int ResultID { get; set; }

        [Required]
        public int EnrolmentID { get; set; }

        [Required]
        public string FinishTime { get; set; } = "00:00:00";

        [Required]
        [Range(1, int.MaxValue)]
        public int FinishPosition { get; set; }

        public int ParticipantID { get; set; }

        public string EventName { get; set; } = "";

        public string CategoryName { get; set; } = "";
    }

    public class ResultCreateViewModel
    {
        [Required]
        public int EnrolmentID { get; set; }

        [Required]
        public string FinishTime { get; set; } = "";

        [Required]
        [Range(1, int.MaxValue)]
        public int FinishPosition { get; set; }

        public List<EnrolmentOptionViewModel> Enrolments { get; set; } = new();
    }

    public class EnrolmentOptionViewModel
    {
        public int EnrolmentID { get; set; }

        public int ParticipantID { get; set; }

        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public string EventName { get; set; } = "";

        public string CategoryName { get; set; } = "";

        public string DisplayName =>
            $"Enrolment #{EnrolmentID} - Participant #{ParticipantID} - {EventName} ({CategoryName})";
    }
}
