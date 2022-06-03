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

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
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
           
            if (await getUserByUsername(username) != null)
            {
                return BadRequest("username exists");
            }
            if (checkEmailValid(email) == false)
            {
                return BadRequest("invalid email");
            }
            if (await getUserByEmail(username) != null)
            {
                return BadRequest("email exists");
            }


            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user = new User();
            user.Username = username;
            user.UserEmail = email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
           
            user.RoleId = GetRoleId();

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            //find user by username, maybe email also
            var loginUser = await getUserByUsername(request.Username);
            if (loginUser == null)
            {
                return BadRequest("User not found");
            }
            user = loginUser;
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("wrong password");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            await SetRefreshToken(refreshToken);
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
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();
            return true;

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
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

        private int GetRoleId(string roleName = "Poster")
        {
            IEnumerable<Role> roleList = _context.Role.ToList();
            foreach (Role role in roleList)
            {
                if (role.Name == roleName)
                {
                    return role.Id;
                }


            }

            return 0;
        }

        private async Task<User?> getUserByUsername(string username)
        {
            var loginUser = await _context.User.Where(s => s.Username == username).Include(t => t.Role).FirstOrDefaultAsync();
            return loginUser;
        }

        private async Task<User?> getUserByEmail(string email)
        {
            var loginUser = await _context.User.Where(s => s.UserEmail == email).FirstOrDefaultAsync();
            if (loginUser == null)
            {
                return null;
            }
            return loginUser;
        }

        private bool checkEmailValid(string email)
        {
            var emailAtt = new EmailAddressAttribute();
            return emailAtt.IsValid(email);
        }
    }
}