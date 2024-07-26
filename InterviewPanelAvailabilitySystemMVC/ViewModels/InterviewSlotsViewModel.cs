using System.ComponentModel.DataAnnotations;

namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class InterviewSlotsViewModel
    {
        [Key]
        public int SlotId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public EmployeesViewModel Employee { get; set; }

        [Required]
        public DateTime SlotDate { get; set; }

        [Required]
        public int TimeslotId { get; set; }

        public TimeslotViewModel Timeslot { get; set; }

        [Required]
        public bool IsBooked { get; set; } = false;
    }
}
