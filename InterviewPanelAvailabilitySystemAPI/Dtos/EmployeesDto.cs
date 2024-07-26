using InterviewPanelAvailabilitySystemAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class EmployeesDto
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

        public JobRole? JobRole { get; set; }

        public int? InterviewRoundId { get; set; }
        public InterviewRounds? InterviewRound { get; set; }
        public bool IsRecruiter { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool ChangePassword { get; set; } = false;

    }
}
