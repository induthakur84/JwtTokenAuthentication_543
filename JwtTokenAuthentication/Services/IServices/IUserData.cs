using JwtTokenAuthentication.DTO;

namespace JwtTokenAuthentication.Services.IServices
{
    public interface IUserData
    {


        Task<UserResponseDto>Register(UserRequestDto userRequestDto);

    }
}
