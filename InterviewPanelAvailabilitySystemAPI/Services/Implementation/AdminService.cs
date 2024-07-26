using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Data.Implementation;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Models;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;
using System.Diagnostics.CodeAnalysis;

namespace InterviewPanelAvailabilitySystemAPI.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPasswordService _passwordService;

        public AdminService(IAdminRepository adminRepository, IPasswordService passwordService)
        {
            _adminRepository = adminRepository;
            _passwordService = passwordService;
        }
        public ServiceResponse<IEnumerable<EmployeesDto>> GetAllEmployees(int page, int pageSize, string? search, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<EmployeesDto>>();
            try
            {
                var users = _adminRepository.GetAllEmployees(page, pageSize, search, sortOrder);
                if (users != null && users.Any())
                {
                    List<EmployeesDto> userDtos = new List<EmployeesDto>();
                    foreach (var interviewSlot in users.ToList())
                    {
                        if (interviewSlot.IsRecruiter)
                        {
                            userDtos.Add(new EmployeesDto()
                            {
                                EmployeeId = interviewSlot.EmployeeId,
                                FirstName = interviewSlot.FirstName,
                                LastName = interviewSlot.LastName,
                                Email = interviewSlot.Email,
                                IsRecruiter = interviewSlot.IsRecruiter,
                                IsAdmin = interviewSlot.IsAdmin,
                                ChangePassword = interviewSlot.ChangePassword
                            });
                        }
                        else
                        {
                            userDtos.Add(new EmployeesDto()
                            {
                                EmployeeId = interviewSlot.EmployeeId,
                                FirstName = interviewSlot.FirstName,
                                LastName = interviewSlot.LastName,
                                Email = interviewSlot.Email,
                                JobRoleId = interviewSlot.JobRoleId,
                                JobRole = new JobRole
                                {
                                    JobRoleId = interviewSlot.JobRole.JobRoleId,
                                    JobRoleName = interviewSlot.JobRole.JobRoleName,
                                },
                                InterviewRoundId = interviewSlot.InterviewRoundId,
                                InterviewRound = new InterviewRounds
                                {
                                    InterviewRoundId = interviewSlot.InterviewRound.InterviewRoundId,
                                    InterviewRoundName = interviewSlot.InterviewRound.InterviewRoundName
                                },
                                IsRecruiter = interviewSlot.IsRecruiter,
                                IsAdmin = interviewSlot.IsAdmin,
                                ChangePassword = interviewSlot.ChangePassword
                            });
                        }
                    }
                    response.Data = userDtos;
                    response.Success = true;
                    response.Message = "Retrived emplopyees successfully!";
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



        //DeleteEmployee
        public ServiceResponse<string> DeleteEmployee(int id)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (id == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
                }
                var updatedEmployee = _adminRepository.GetEmployeeById(id);
                if (updatedEmployee != null)
                {
                    updatedEmployee.IsActive = false;
                    var result = _adminRepository.UpdateEmployee(updatedEmployee);

                    response.Success = result;
                    response.Message = result ? "Employee deleted successfully." : "Something went wrong. Please try after sometime.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Employee not found.";
                    return response;
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


        //UpdateEmployee
        public ServiceResponse<string> UpdateEmployee(UpdateEmployeeDtos employees)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (employees == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
                }
                if (AlreadyExists(employees.EmployeeId, employees.Email))
                {
                    response.Success = false;
                    response.Message = "Employee already exists.";
                    return response;
                }
                var updatedEmployee = _adminRepository.GetEmployeeById(employees.EmployeeId);
                if (updatedEmployee != null )
                {
                    if(updatedEmployee.IsAdmin == true)
                    {
                        response.Success = false;
                        response.Message = "Admin can't updated.";
                        return response;
                    }
                    updatedEmployee.FirstName = employees.FirstName;
                    updatedEmployee.LastName = employees.LastName;
                    updatedEmployee.Email = employees.Email;
                    updatedEmployee.JobRoleId = employees.JobRoleId;
                    updatedEmployee.InterviewRoundId = employees.InterviewRoundId;

                    var result = _adminRepository.UpdateEmployee(updatedEmployee);

                    response.Success = result;
                    response.Message = result ? "Employee updated successfully." : "Something went wrong. Please try after sometime.";
                
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
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


        //Update - AlreadyExists
        [ExcludeFromCodeCoverage]
        private bool AlreadyExists(int employeeId, string email)
        {
            var result = false;
            try
            {
                var employee = _adminRepository
                    .GetEmployeeByEmployeeIdAndEmail(employeeId, email);
                if (employee != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        //GetEmployeeById
        public ServiceResponse<EmployeesDto> GetEmployeeById(int id)
        {
            var response = new ServiceResponse<EmployeesDto>();
            try
            {
                var employee = _adminRepository.GetEmployeeById(id);
                if (employee != null)
                {
                    if (employee.JobRoleId == null)
                    {
                        var employeeDto = new EmployeesDto()
                        {
                            EmployeeId = employee.EmployeeId,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            IsRecruiter = employee.IsRecruiter,
                            IsAdmin = employee.IsAdmin,
                            ChangePassword = employee.ChangePassword
                        };
                        response.Data = employeeDto;
                    }
                    else
                    {
                        var employeeDto = new EmployeesDto()
                        {
                            EmployeeId = employee.EmployeeId,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            IsRecruiter = employee.IsRecruiter,
                            IsAdmin = employee.IsAdmin,
                            ChangePassword = employee.ChangePassword,
                            JobRoleId = employee.JobRoleId,
                            InterviewRoundId = employee.InterviewRoundId,
                            JobRole = new JobRole()
                            {
                                JobRoleId = employee.JobRole.JobRoleId,
                                JobRoleName = employee.JobRole.JobRoleName,
                            },
                            InterviewRound = new InterviewRounds()
                            {
                                InterviewRoundId = employee.InterviewRound.InterviewRoundId,
                                InterviewRoundName = employee.InterviewRound.InterviewRoundName,
                            }
                        };
                        response.Data = employeeDto;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong,try after sometime";
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
        public ServiceResponse<int> TotalEmployeeCount(string? search)
        {
            var response = new ServiceResponse<int>();
            try
            {
                int totalUsers = _adminRepository.TotalEmployeeCount(search);

                response.Data = totalUsers;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ServiceResponse<string> AddEmployee(AddEmployeeDto register)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var password = string.Empty;
                if (register != null)
                {
                    if (_adminRepository.EmployeeExists(register.Email))
                    {
                        response.Success = false;
                        response.Message = "Interviewer already exist";
                        return response;
                    }
                    if (register.FirstName != null)
                    {
                        password = char.ToUpper(register.FirstName[0]) + register.FirstName.Substring(1) + "@123";
                    }
                    Employees employee = new Employees()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        JobRoleId = register.JobRoleId,
                        InterviewRoundId = register.InterviewRoundId,
                        IsActive = true,
                        IsAdmin = false,
                        IsRecruiter = false,
                        ChangePassword = false,
                    };

                    _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                    employee.PasswordHash = passwordHash;
                    employee.PasswordSalt = passwordSalt;
                    var result = _adminRepository.AddEmployee(employee);
                    if (result)
                    {
                        response.Success = true;
                        response.Message = "Interviewer created successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Something went wrong, please try after sometime";
                    }
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
        public ServiceResponse<IEnumerable<JobRoleDto>> GetAllJobRoles()
        {
            var response = new ServiceResponse<IEnumerable<JobRoleDto>>();
            try
            {
                var jobRoles = _adminRepository.GetAllJobRoles();

                if (jobRoles != null && jobRoles.Any())
                {
                    var jobRoleDtoList = new List<JobRoleDto>();
                    foreach (var jobRole in jobRoles)
                    {
                        jobRoleDtoList.Add(new JobRoleDto()
                        {
                            JobRoleId = jobRole.JobRoleId,
                            JobRoleName = jobRole.JobRoleName,
                        });
                    }
                    response.Data = jobRoleDtoList;
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
        public ServiceResponse<IEnumerable<InterviewRoundsDto>> GetAllInterviewRounds()
        {
            var response = new ServiceResponse<IEnumerable<InterviewRoundsDto>>();
            try
            {
                var interviewRounds = _adminRepository.GetAllInterviewRounds();

                if (interviewRounds != null && interviewRounds.Any())
                {
                    var interviewRoundsDtoList = new List<InterviewRoundsDto>();
                    foreach (var interviewRound in interviewRounds)
                    {
                        interviewRoundsDtoList.Add(new InterviewRoundsDto()
                        {
                            InterviewRoundId = interviewRound.InterviewRoundId,
                            InterviewRoundName = interviewRound.InterviewRoundName,
                        });
                    }
                    response.Data = interviewRoundsDtoList;
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
        public ServiceResponse<bool> GetIsChangedPasswordById(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var employee = _adminRepository.GetEmployeeById(id);
                if (employee != null)
                {
                    if (employee.ChangePassword == false)
                    {
                        response.Data = false;
                        response.Success = true;
                        response.Message = "ChangePassword is false";
                    }
                    else
                    {
                        response.Data = true;
                        response.Success = true;
                        response.Message = "ChangePassword is true";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong,try after sometime";
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
