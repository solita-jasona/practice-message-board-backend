using System;
namespace MessageBoardBackend.Services
{
	public interface IUserService
	{
		Task<User?> GetUserByUsername(string username);

		Task<User?> GetUserByEmail(string email);

		Task<int> GetRoleId(string roleName = "Poster");

		bool ValidateEmail(string email);

		Task<bool> AddUser(User user);

		Task<bool> UpdateUser(User user);

        Task<User?> GetUser(int id);

    }
}

