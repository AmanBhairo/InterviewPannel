using InterviewPanelAvailabilitySystemAPI.Models;

namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class RecruiterDetailsDto
    {
        public int EmployeeId { get; set; }
        public int TimeslotId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime SlotDate { get; set; }
        public int SlotId { get; set; }
        public bool IsBooked { get; set; } = false;
        public int? JobRoleId { get; set; }
        public string JobRoleName { get; set; }
        public string TimeslotName { get; set; }
        public int? InterviewRoundId { get; set; }
        public string InterviewRoundName { get; set; }
    }
}
