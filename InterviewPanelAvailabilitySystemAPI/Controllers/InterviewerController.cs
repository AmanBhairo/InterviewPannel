using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPanelAvailabilitySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class InterviewerController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewerController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [Authorize(Policy = "InterviewerPolicy")]
        [HttpPost("AddInterviewSlot")]
        public IActionResult AddInterviewSlot(AddInterviewSlotsDto slotDto)
        {
            try
            {
                var slot = new InterviewSlots()

                {
                    EmployeeId = slotDto.EmployeeId,
                    SlotDate = slotDto.SlotDate.Date,
                    TimeslotId = slotDto.TimeslotId,
                    IsBooked = false,

                };

                var result = _interviewService.AddInterviewSlot(slot);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [Authorize(Policy = "InterviewerPolicy")]
        [HttpDelete("DeleteInterviewSlot{id}")]
        public IActionResult DeleteInterviewSlot(int id, DateTime slotDate,int timeSlotId)
        {
            try
            {
                if (id > 0)
                {
                    var response = _interviewService.RemoveSlot(id, slotDate, timeSlotId);
                    if (!response.Success)
                    {
                        return BadRequest(response);
                    }
                    else
                    {
                        return Ok(response);
                    }
                }
                else
                {
                    return BadRequest("Enter correct data please");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }

        [Authorize(Policy = "InterviewerPolicy")]
        [HttpGet("GetAllTimeslots")]
        public IActionResult GetAllTimeslots()
        {
            try
            {
                var response = _interviewService.GetAllTimeslots();
                if (!response.Success)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [Authorize(Policy = "InterviewerPolicy")]
        [HttpGet("GetAllInterviewslots")]
        public IActionResult GetAllInterviewslotsByEmployeeId(int employeeId)
        {
            try
            {

                var response = _interviewService.GetAllInterviewslotsbyEmployeeId(employeeId);
                if (!response.Success)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
