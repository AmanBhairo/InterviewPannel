using InterviewPanelAvailabilitySystemAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class InterviewSlotsDto
    {
        [Key]
        public int SlotId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employees Employee { get; set; }

        [Required]
        public DateTime SlotDate { get; set; }

        [Required]
        public int TimeslotId { get; set; }

        public Timeslot Timeslot { get; set; }

        [Required]
        public bool IsBooked { get; set; } = false;
    }
}
