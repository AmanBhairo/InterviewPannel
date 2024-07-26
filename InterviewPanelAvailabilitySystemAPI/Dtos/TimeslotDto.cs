using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class TimeslotDto
    {
        [Required]
        public int TimeslotId { get; set; }

        [Required]
        public string TimeslotName { get; set; }

    }
}
