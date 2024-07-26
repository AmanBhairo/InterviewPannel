using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Models
{
    public class JobRole
    {
        [Key]
        [Required]
        public int JobRoleId { get; set; }

        [Required]
        public string JobRoleName { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }

    }
}
