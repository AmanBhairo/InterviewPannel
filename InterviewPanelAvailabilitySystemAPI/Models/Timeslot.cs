using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Models
{
    public class Timeslot
    {
        [Key]
        [Required]
        public int TimeslotId { get; set; }

        [Required]
        public string TimeslotName { get; set; }

        public virtual ICollection<InterviewSlots> InterviewSlots { get; set; }

    }
}
