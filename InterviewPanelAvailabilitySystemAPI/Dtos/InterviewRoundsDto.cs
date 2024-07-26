using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class InterviewRoundsDto
    {
        [Required]
        public int InterviewRoundId { get; set; }

        [Required]
        public string InterviewRoundName { get; set; }
    }
}
