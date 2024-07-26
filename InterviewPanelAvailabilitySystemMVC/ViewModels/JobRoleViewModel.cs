using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class JobRoleViewModel
    {
        //[Required(ErrorMessage = "Job role id is required")]
        //[DisplayName("Job Role Id")]
        public int JobRoleId { get; set; }

        //[Required(ErrorMessage = "Job role name is required")]
        //[DisplayName("Job Role Name")]
        public string JobRoleName { get; set; }
    }
}
