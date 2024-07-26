using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class InterviewRoundViewModel
    {
        [Required(ErrorMessage = "Interview round id is required")]
        [DisplayName("Interview Round Id")]
        public int InterviewRoundId { get; set; }

        [Required(ErrorMessage = "Interview round name is required")]
        [DisplayName("Interview Round Name")]
        public string InterviewRoundName { get; set; }
    }
}
