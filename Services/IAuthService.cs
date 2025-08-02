using LoginAPI.Models;
using LoginAPI.DTOs;

namespace LoginAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<BaseResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<BaseResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        string GenerateJwtToken(User user);
    }
}