using LoginAPI.DTOs;
using LoginAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Dados inválidos"
                    });
                }

                var result = await _authService.LoginAsync(loginDto);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                _logger.LogInformation("Successful login for user: {Email}", loginDto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Email}", loginDto.Email);
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Dados inválidos"
                    });
                }

                var result = await _authService.RegisterAsync(registerDto);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                _logger.LogInformation("Successful registration for user: {Email}", registerDto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user: {Email}", registerDto.Email);
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<BaseResponseDto>> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponseDto
                    {
                        Success = false,
                        Message = "Email inválido"
                    });
                }

                var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);

                _logger.LogInformation("Password reset requested for email: {Email}", forgotPasswordDto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during forgot password for email: {Email}", forgotPasswordDto.Email);
                return StatusCode(500, new BaseResponseDto
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<BaseResponseDto>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponseDto
                    {
                        Success = false,
                        Message = "Dados inválidos"
                    });
                }

                var result = await _authService.ResetPasswordAsync(resetPasswordDto);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                _logger.LogInformation("Successful password reset with token: {Token}", resetPasswordDto.Token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset with token: {Token}", resetPasswordDto.Token);
                return StatusCode(500, new BaseResponseDto
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetAuthenticatedUser()
        {
            var email = User.Identity?.Name ?? "desconhecido";
            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = $"Usuário autenticado: {email}",
                Token = null
            });
        }
    }
}