using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Models
{
    public class InterviewRounds
    {
        [Key]
        [Required]
        public int InterviewRoundId { get; set; }

        [Required]
        public string InterviewRoundName { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }

    }
}
