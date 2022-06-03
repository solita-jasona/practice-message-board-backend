using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MessageBoardBackend.Controllers
{
    public class AuthController : Controller
    {
        public static User user = new User();

        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            string username = request.Username;
            string email = request.UserEmail;
            string password = request.Password;
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                return BadRequest("missing fields");
            }
           
            if (await _userService.GetUserByUsername(username) != null)
            {
                return BadRequest("username exists");
            }
            if (_userService.ValidateEmail(email) == false)
            {
                return BadRequest("invalid email");
            }
            if (await _userService.GetUserByEmail(email) != null)
            {
                return BadRequest("email exists");
            }


            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user = new User();
            user.Username = username;
            user.UserEmail = email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
           
            user.RoleId = await _userService.GetRoleId();

            if (await _userService.AddUser(user))
            {
                return Ok(user);
            }
            return BadRequest("User registration failed");
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            //find user by username, maybe email also
            var loginUser = await _userService.GetUserByUsername(request.Username);
            if (loginUser == null)
            {
                loginUser = await _userService.GetUserByEmail(request.Username);
                if (loginUser == null)
                {
                    return BadRequest("User not found or wrong password");
                }    
            }
            user = loginUser;
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("User not found or wrong password");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            if (!await SetRefreshToken(refreshToken))
            {
                return BadRequest("Refresh token failed to save to DB");
            }
            
            var tokens = new { Token = token, RefreshToken = refreshToken, UserId = user.Id, username = user.Username, UserEmail = user.UserEmail, Role = user.Role.Name };

            return Ok(tokens);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async Task<bool> SetRefreshToken(RefreshToken newRefreshToken)
        {
            user.RefreshToken = newRefreshToken;
            return await _userService.UpdateUser(user);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}