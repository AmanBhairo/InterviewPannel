using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPanelAvailabilitySystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees(string? search, int page = 1, int pageSize = 4, string sortOrder = "asc")
        {
            try
            {
                var response = _adminService.GetAllEmployees(page, pageSize, search, sortOrder);
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee(AddEmployeeDto employeesDto)
        {
            try
            {
                var response = _adminService.AddEmployee(employeesDto);
                return !response.Success ? BadRequest(response) : Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTotalEmployeeCount")]
        public IActionResult GetTotalEmployeeCount(string? search)
        {
            try
            {
                var response = _adminService.TotalEmployeeCount(search);
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

        [Authorize]
        [HttpGet("GetAllJobRoles")]
        public IActionResult GetAllJobRoles()
        {
            try
            {
                var response = _adminService.GetAllJobRoles();
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

            [Authorize]
        [HttpGet("GetAllInterviewRounds")]
        public IActionResult GetAllInterviewRounds()
        {
            try
            {
                var response = _adminService.GetAllInterviewRounds();
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
        //RemoveEmployee
        [HttpPut("RemoveEmployee")]
        [Authorize(Policy = "AdminPolicy")]
        //[HttpDelete("RemoveEmployee")]
        public IActionResult RemoveEmployee(int id)
        {
            try
            {
                var response = _adminService.DeleteEmployee(id);

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

        //EditEmployee
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("EditEmployee")]
        public IActionResult Edit(UpdateEmployeeDtos updateEmployeeDto)
        {
            try
            {
                var employees = new UpdateEmployeeDtos()
                {
                    EmployeeId = updateEmployeeDto.EmployeeId,
                    FirstName = updateEmployeeDto.FirstName,
                    LastName = updateEmployeeDto.LastName,
                    Email = updateEmployeeDto.Email,
                    JobRoleId = updateEmployeeDto.JobRoleId,
                    InterviewRoundId = updateEmployeeDto.InterviewRoundId,

                };
                var result = _adminService.UpdateEmployee(employees);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GetEmployeeById
        [HttpGet("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                var response = _adminService.GetEmployeeById(id);
                if (!response.Success)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetIsChangedPasswordById/{id}")]
        public IActionResult GetIsChangedPasswordById(int id)
        {
            try
            {
                var response = _adminService.GetIsChangedPasswordById(id);
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
