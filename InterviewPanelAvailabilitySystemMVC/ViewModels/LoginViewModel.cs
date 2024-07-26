using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is requried")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
