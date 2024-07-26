using InterviewPanelAvailabilitySystemAPI.Dtos;

namespace InterviewPanelAvailabilitySystemAPI.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<string> LoginUserService(LoginDto login);
        ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto);
    }
}
