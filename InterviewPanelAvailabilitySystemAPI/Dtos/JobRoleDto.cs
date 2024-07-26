using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class JobRoleDto
    {
        [Required]
        public int JobRoleId { get; set; }

        [Required]
        public string JobRoleName { get; set; }
    }
}
