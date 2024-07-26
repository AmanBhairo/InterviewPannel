namespace InterviewPanelAvailabilitySystemMVC.ViewModels
{
    public class DetailedReportViewModel
    {
        public int EmployeeId { get; set; }
        public int TimeslotId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime SlotDate { get; set; }
        public string timeSlotName { get; set; }
        public List<JobRoleViewModel>? JobRoles { get; set; }
        public List<InterviewRoundViewModel>? InterviewRounds { get; set; }
        public int? JobRoleId { get; set; }
        public int? InterViewRoleId { get; set; }
    }
}
