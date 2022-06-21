using System;
using System.ComponentModel.DataAnnotations;

namespace MessageBoardBackend.Services
{
	public class UserService : IUserService
	{
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> GetRoleId(string roleName = "Poster")
        {
            var role = await _context.Role.Where(s => s.Name == roleName).FirstOrDefaultAsync();
            if (role != null)
            {
                return role.Id;
            }
            return 0;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.User.Where(s => s.UserEmail == email).Include(t => t.Role).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.User.Where(s => s.Username == username).Include(t => t.Role).FirstOrDefaultAsync();
        }

        public bool ValidateEmail(string email)
        {
            var emailAtt = new EmailAddressAttribute();
            return emailAtt.IsValid(email);
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<bool> UpdateUser(User user)
        {

            try
            {
                var dbUser = await _context.User.FindAsync(user.Id);
                if (dbUser == null)
                {
                    return false;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<User?> GetUser(int id)
        {
            User? user = await _context.User.Include("Role").FirstOrDefaultAsync(i => i.Id == id);
            if (user == default(User))
            {
                return null;
            }
            return user;
        }

    }
}

