using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace InterviewPanelAvailabilitySystemAPI.Services.Implementation
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IRecruiterRepository _recruiterRepository;

        public RecruiterService(IRecruiterRepository recruiterRepository)
        {
            _recruiterRepository = recruiterRepository;
        }

      
        public ServiceResponse<InterviewSlotsDto> GetInterviewSlotsById(int slotId)
        {
            var response = new ServiceResponse<InterviewSlotsDto>();
            try
            {
                var interviewSlot = _recruiterRepository.GetInterviewSlotsById(slotId);
                if (interviewSlot == null)
                {
                    response.Success = false;
                    response.Message = "No records found.";
                    return response;
                }
                var interviewSlotDtos = new InterviewSlotsDto()
                {
                    EmployeeId = interviewSlot.EmployeeId,
                    SlotDate = interviewSlot.SlotDate,
                    SlotId = interviewSlot.SlotId,

                    Employee = new Employees()
                    {
                        FirstName = interviewSlot.Employee.FirstName,
                        LastName = interviewSlot.Employee.LastName,
                        JobRoleId = interviewSlot.Employee.JobRoleId,
                        Email = interviewSlot.Employee.Email,
                        InterviewRoundId = interviewSlot.Employee.InterviewRoundId,
                        IsActive = interviewSlot.Employee.IsActive,
                        IsAdmin = interviewSlot.Employee.IsAdmin,
                        IsRecruiter = interviewSlot.Employee.IsRecruiter,
                        InterviewRound = new InterviewRounds()
                        {
                            InterviewRoundId = interviewSlot.Employee.InterviewRound.InterviewRoundId,
                            InterviewRoundName = interviewSlot.Employee.InterviewRound.InterviewRoundName

                        },
                        JobRole = new JobRole()
                        {
                            JobRoleName = interviewSlot.Employee.JobRole.JobRoleName
                        }
                    },
                    Timeslot = new Timeslot()
                    {
                        TimeslotId = interviewSlot.TimeslotId,
                        TimeslotName = interviewSlot.Timeslot.TimeslotName,
                    }
                };


                response.Data = interviewSlotDtos;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ServiceResponse<string> ModifyInterviewSlots(InterviewSlots slots)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (_recruiterRepository.InterviewSlotExist(slots.SlotId, slots.IsBooked))
                {
                    response.Success = false;
                    response.Message = "Interview Slot Already Booked";
                    return response;
                }

                var existingInterviewSlots = _recruiterRepository.GetInterviewSlotsById(slots.SlotId);
                var result = false;
                if (existingInterviewSlots != null)
                {
                    existingInterviewSlots.IsBooked = true;
                    result = _recruiterRepository.UpdateInterviewSlots(existingInterviewSlots);
                }

                if (result)
                {
                    response.Message = "Interview slot booked successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after sometime.";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse<IEnumerable<InterviewSlotsDto>> GetPaginatedInterviwerByAll(int page, int pageSize, string? searchQuery, string sortOrder,int? jobRoleId,int? roundId)
        {
            var response = new ServiceResponse<IEnumerable<InterviewSlotsDto>>();
            try
            {
                var interviewSlots = _recruiterRepository.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

                if (interviewSlots != null && interviewSlots.Any())
                {
                    List<InterviewSlotsDto> interviewSlotsDtos = new List<InterviewSlotsDto>();
                    foreach (var interviewSlot in interviewSlots.ToList())
                    {
                        interviewSlotsDtos.Add(new InterviewSlotsDto()
                        {
                            EmployeeId = interviewSlot.EmployeeId,
                            SlotDate = interviewSlot.SlotDate,
                            SlotId = interviewSlot.SlotId,

                            Employee = new Employees()
                            {
                                FirstName = interviewSlot.Employee.FirstName,
                                LastName = interviewSlot.Employee.LastName,
                                JobRoleId = interviewSlot.Employee.JobRoleId,
                                Email = interviewSlot.Employee.Email,
                                InterviewRoundId = interviewSlot.Employee.InterviewRoundId,
                                IsActive = interviewSlot.Employee.IsActive,
                                IsAdmin = interviewSlot.Employee.IsAdmin,
                                IsRecruiter = interviewSlot.Employee.IsRecruiter,
                                InterviewRound = new InterviewRounds()
                                {
                                    InterviewRoundId = interviewSlot.Employee.InterviewRound.InterviewRoundId,
                                    InterviewRoundName = interviewSlot.Employee.InterviewRound.InterviewRoundName

                                },
                                JobRole = new JobRole()
                                {
                                    JobRoleName = interviewSlot.Employee.JobRole.JobRoleName
                                }
                            },
                            Timeslot = new Timeslot()
                            {
                                TimeslotId = interviewSlot.TimeslotId,
                                TimeslotName = interviewSlot.Timeslot.TimeslotName,
                            }
                        });
                    }


                    response.Data = interviewSlotsDtos;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse<int> TotalInterviewSlotsByAll(string? searchQuery, int? jobRoleId, int? roundId)
        {
            var response = new ServiceResponse<int>();
            try
            {
                int totalInterviewSlots = _recruiterRepository.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId);

                response.Data = totalInterviewSlots;
                response.Success = true;
                response.Message = "Pagination successful";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
