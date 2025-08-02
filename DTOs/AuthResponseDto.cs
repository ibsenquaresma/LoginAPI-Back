namespace LoginAPI.DTOs
{
    public class AuthResponseDto : BaseResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}