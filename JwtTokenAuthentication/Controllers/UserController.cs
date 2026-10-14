using JwtTokenAuthentication.DTO;
using JwtTokenAuthentication.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserData _userData;
        public UserController(IUserData userData)
        {
            _userData = userData;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(UserRequestDto requestDto)
        {
            var result= await _userData.Register(requestDto);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var result = await _userData.Login(loginRequestDto);

                return Ok(
                    new
                    {
                        message = "Login Successfully",
                        token = result.Token,
                        user = result.User
                    }
                    );
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Login Failed",
                    error = ex.Message,
                });
            }
        }
    }
}
