using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using InterviewPanelAvailabilitySystemAPI.Services.Infrastructure;

namespace InterviewPanelAvailabilitySystemAPI.Services.Implementation
{

    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepository;

        public InterviewService(IInterviewRepository interviewRepository)
        {
            _interviewRepository = interviewRepository;

        }
        public ServiceResponse<string> AddInterviewSlot(InterviewSlots slots)
        {
            var response = new ServiceResponse<string>();

            try
            {
                if (_interviewRepository.InterviewSlotExist(slots.EmployeeId, slots.SlotDate, slots.TimeslotId))
                {
                    response.Success = false;
                    response.Message = "Interview Slot Already Booked";
                    return response;
                }



                if (slots.SlotDate < DateTime.Now.Date.AddDays(1))
                {
                    response.Success = false;
                    response.Message = "Can't book slots for past date";
                    return response;
                }

                if (slots.SlotDate > DateTime.Now.Date.AddDays(10))
                {
                    response.Success = false;
                    response.Message = "Can't book slots for after 10 days";
                    return response;
                }

                var result = _interviewRepository.AddInterviewSlot(slots);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Slot Saved Successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try later";
                }
                return response;
            }
            catch(Exception ex) 
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse<string> RemoveSlot(int id, DateTime slotDate, int timeSlotId)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var result = _interviewRepository.DeleteInterviewSlot(id, slotDate, timeSlotId);

                if (result)
                {
                    response.Message = "Slot deleted successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong";
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

        public ServiceResponse<IEnumerable<TimeslotDto>> GetAllTimeslots()
        {
            var response = new ServiceResponse<IEnumerable<TimeslotDto>>();
            try
            {
                var timeslots = _interviewRepository.GetAllTimeslots();

                if (timeslots != null && timeslots.Any())
                {
                    var timeslotDtoList = new List<TimeslotDto>();
                    foreach (var timeslot in timeslots)
                    {
                        timeslotDtoList.Add(new TimeslotDto()
                        {
                            TimeslotId = timeslot.TimeslotId,
                            TimeslotName = timeslot.TimeslotName,
                        });
                    }
                    response.Data = timeslotDtoList;
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
         public ServiceResponse<IEnumerable<InterviewSlotsDto>> GetAllInterviewslotsbyEmployeeId(int employeeId)
        {
            var response = new ServiceResponse<IEnumerable<InterviewSlotsDto>>();
            try
            {
                var interviewSlots = _interviewRepository.GetAllInterviewslotsbyEmployeeId(employeeId);

                if (interviewSlots != null && interviewSlots.Any())
                {
                    var interviewslotDtoList = new List<InterviewSlotsDto>();
                    foreach (var interviewSlot in interviewSlots)
                    {
                        interviewslotDtoList.Add(new InterviewSlotsDto()
                        {
                            SlotId = interviewSlot.SlotId,
                            EmployeeId = employeeId,
                            TimeslotId = interviewSlot.TimeslotId,
                            IsBooked = interviewSlot.IsBooked,
                            SlotDate = interviewSlot.SlotDate
                        });
                    }
                    response.Data = interviewslotDtoList;
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


    }
}
