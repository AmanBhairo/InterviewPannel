using InterviewPanelAvailabilitySystemAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class AddInterviewSlotsDto
    {
       
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime SlotDate { get; set; }

        [Required]
        public int TimeslotId { get; set; }


        [Required]
        public bool IsBooked { get; set; } = false;
    }
}
