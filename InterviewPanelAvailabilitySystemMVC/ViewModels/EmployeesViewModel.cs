using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class EmployeesViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public int? JobRoleId { get; set; }

        public JobRoleViewModel? JobRole { get; set; }

        public int? InterviewRoundId { get; set; }
        public InterviewRoundViewModel? InterviewRound { get; set; }
        public bool IsRecruiter { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool ChangePassword { get; set; } = false;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<InterviewSlotsViewModel> InterviewSlots { get; set; }

    }
}
