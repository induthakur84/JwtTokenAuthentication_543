using JwtTokenAuthentication.DTO;

namespace JwtTokenAuthentication.Services.IServices
{
    public interface IUserData
    {

        // add a new user inside the database
        Task<UserResponseDto>Register(UserRequestDto userRequestDto);


        // user method where user will login with valid username and password 
        // then we return token.

        Task<LoginResponseDto>Login(LoginRequestDto loginRequestDto);
    }
}
