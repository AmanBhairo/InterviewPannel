using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace InterviewPanelAvailabilitySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruiterController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;

        public RecruiterController(IRecruiterService recruiterService)
        {
            _recruiterService = recruiterService;
        }

        [Authorize(Policy = "RecruiterPolicy")]
        [HttpGet("GetInterviewSlotsById/{slotId}")]
        public IActionResult GetInterviewSlotsById(int slotId)
        {
            try
            {
                var response = _recruiterService.GetInterviewSlotsById(slotId);
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

        [Authorize(Policy = "RecruiterPolicy")]
        [HttpPut("UpdateInterviewSlot")]
        public IActionResult UpdateInterviewSlot(UpdateInterviewSlotsDto slotDto)
        {
            try
            {
                var slot = new InterviewSlots()
                {
                    SlotId = slotDto.SlotId,

                };
                var response = _recruiterService.ModifyInterviewSlots(slot);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "RecruiterPolicy")]
        [HttpGet("GetPaginatedInterviwerByAll")]
        public IActionResult GetPaginatedInterviwerByAll(int page = 1, int pageSize = 20, string? searchQuery = "", string sortOrder = "asc",int? jobRoleId=null,int? roundId=null)
        {
            try
            {
                var response = new ServiceResponse<IEnumerable<InterviewSlotsDto>>();
                response = _recruiterService.GetPaginatedInterviwerByAll(page, pageSize, searchQuery, sortOrder, jobRoleId, roundId);

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

        [HttpGet("GetTotalInterviewSlotsByAll")]
        public IActionResult GetTotalInterviewSlotsByAll(string? searchQuery, int? jobRoleId, int? roundId)
        {
            try
            {
                var response = _recruiterService.TotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId);
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

