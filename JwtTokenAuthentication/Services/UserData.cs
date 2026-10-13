using JwtTokenAuthentication.DTO;
using JwtTokenAuthentication.Services.IServices;

namespace JwtTokenAuthentication.Services
{
    public class UserData : IUserData
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UserData(ApplicationDbContext context, 
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<UserResponseDto> Register(UserRequestDto userRequestDto)
        {
            // here we can mannual mapping
            // map the userrequest dto to the user entity
            var user = new User
            {
                FirstName = userRequestDto.FirstName,
                LastName = userRequestDto.LastName,
                Email = userRequestDto.Email,
                Password = userRequestDto.Password,
                ConfirmPassword = userRequestDto.ConfirmPassword,
                Role=userRequestDto.Role
            };


            //add user to the database

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();


            return new UserResponseDto
            {

                FirstName= userRequestDto.FirstName,
                LastName= userRequestDto.LastName,
                Email= userRequestDto.Email,
                UserName= userRequestDto.UserName,
            };
        }
    }
}
