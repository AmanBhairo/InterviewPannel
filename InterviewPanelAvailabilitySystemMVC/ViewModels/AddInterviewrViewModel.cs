using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class AddInterviewrViewModel
    {
        [Required(ErrorMessage = "First name is requried")]
        [DisplayName("First name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is requried")]
        [DisplayName("Last name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is requried")]
        [DisplayName("Email")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [StringLength(50)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Jobe role is requried")]
        [DisplayName("Jobe role")]
        public int? JobRoleId { get; set; }
        [Required(ErrorMessage = "Interview round is requried")]
        [DisplayName("Interview round")]
        public int? InterviewRoundId { get; set; }
        public IEnumerable<JobRoleViewModel>? JobRoles { get; set; }
        public IEnumerable<InterviewRoundViewModel>? InterviewRounds { get; set; }
    }
}
