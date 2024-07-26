using InterviewPanelAvailabilitySystemAPI.Data.Contract;
using InterviewPanelAvailabilitySystemAPI.Dtos;
using InterviewPanelAvailabilitySystemAPI.Services.Contract;

namespace InterviewPanelAvailabilitySystemAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordService _passwordService;

        public AuthService(IAuthRepository authRepository, IPasswordService passwordService)
        {
            _authRepository = authRepository;
            _passwordService = passwordService;
        }
        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (login != null)
                {
                    var user = _authRepository.ValidateUser(login.Username);
                    if (user == null)
                    {
                        response.Success = false;
                        response.Message = "Invalid user email or password!";
                        return response;
                    }
                    if (!user.IsActive)
                    {
                        response.Success = false;
                        response.Message = "Invalid user email or password!";
                        return response;
                    }
                    if (!_passwordService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        response.Success = false;
                        response.Message = "Invalid user email or password!";
                        return response;
                    }
                    string token = _passwordService.CreateToken(user);
                    response.Success = true;
                    response.Data = token;
                    response.Message = "Success";
                    return response;
                }
                response.Success = false;
                response.Message = "Something went wrong, please try after some time";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var message = string.Empty;
                if (changePasswordDto != null)
                {
                    var user = _authRepository.ValidateUser(changePasswordDto.Email);
                    if (user == null)
                    {
                        response.Success = false;
                        response.Message = "Invalid email or password";
                        return response;
                    }
                    if (!_passwordService.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                    {
                        response.Success = false;
                        response.Message = "Old password is incorrect.";
                        return response;
                    }
                    if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
                    {
                        response.Success = false;
                        response.Message = "Old password and new password can not be same.";
                        return response;
                    }
                    message = _passwordService.CheckPasswordStrength(changePasswordDto.NewPassword);
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        response.Success = false;
                        response.Message = message;
                        return response;
                    }
                    if (!user.ChangePassword)
                    {
                        user.ChangePassword = true;
                    }
                    _passwordService.CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.UpdateUser(user);
                    response.Success = result;
                    if (result)
                    {
                        response.Message = "Successfully changed password, Please signin!";
                    }
                    else
                    {
                        response.Message = "Something went wrong, please try after sometime";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after sometime";
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
