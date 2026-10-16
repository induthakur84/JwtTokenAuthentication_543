using JwtTokenAuthentication.DTO;
using JwtTokenAuthentication.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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


                // we will never save plain password admin

                //Bcrypt.HashPasswordL convert password admin to hash 'akkjsdfjkds#'
               


                Password = BCrypt.Net.BCrypt.HashPassword(userRequestDto.Password),
                ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(userRequestDto.ConfirmPassword),
                Role = userRequestDto.Role,
                UserName = userRequestDto.UserName,
            };


            //add user to the database

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();


            return new UserResponseDto
            {

                FirstName = userRequestDto.FirstName,
                LastName = userRequestDto.LastName,
                Email = userRequestDto.Email,
                UserName = userRequestDto.UserName,
            };
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
           var user= _context.Users.FirstOrDefault(u=>u.UserName==loginRequestDto.UserName);

            if (user == null)
            {
                throw new Exception("User not found in the database");
            }

            //Admin

            // 'adasdgsd'
            bool isValid= BCrypt.Net.BCrypt.Verify(loginRequestDto.Password,user.Password);

            if (!isValid)
            {

                throw new Exception("Invalid Password");
            }

            var token = GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                User = new UserResponseDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                }
            };
        }


        /// private method
        /// 
        #region private method

        private string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");


            //convert key to bytes and generate  secruity key


            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (jwtSettings["SecretKey"]));

            //create siging crediential using key +algorithim

            var creds= new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);


            //payload
            // here we can save the user claim and userinformation that we want to save inside the token.

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastNAme", user.LastName),

            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claim,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials:creds
                





                );


            // convert token object to string and return

            return new JwtSecurityTokenHandler().WriteToken(token);

           
        }
        #endregion

    }
}
