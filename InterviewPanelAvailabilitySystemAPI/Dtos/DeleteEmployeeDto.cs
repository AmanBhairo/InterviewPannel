using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class DeleteEmployeeDto
    {
        [Key]
        public int EmployeeId { get; set; }
    }
}
