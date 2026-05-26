using LTS.Common.Interfaces;
using LTS.Common.Models;
using System;

namespace LTS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public void Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username required");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password required");

            if (user.Password.Length < 6)
                throw new Exception("Password must be at least 6 characters");

            var existing = _repo.GetByUsername(user.Username);
            if (existing != null)
                throw new Exception("Username already exists");

            user.IsActive = true;

            _repo.Add(user);
        }

        public User Login(string username, string password)
        {
            var user = _repo.GetByUsername(username);

            if (user == null || user.Password != password || !user.IsActive)
                return null;

            return user;
        }
    }
}