using InterviewPanelAvailabilitySystemAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class UpdateEmployeeDtos
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
        public int? InterviewRoundId { get; set; }
    }
}
