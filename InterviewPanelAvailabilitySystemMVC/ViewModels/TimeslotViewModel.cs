using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class TimeslotViewModel
    {
        [Required]
        public int TimeslotId { get; set; }

        [Required]
        public string TimeslotName { get; set; }
    }
}
