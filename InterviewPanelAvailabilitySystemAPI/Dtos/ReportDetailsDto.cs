namespace InterviewPanelAvailabilitySystemAPI.Dtos
{
    public class ReportDetailsDto
    {
        public int EmployeeId { get; set; }
        public int TimeslotId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime SlotDate { get; set; }
        public string timeSlotName { get; set; }



    }
}
