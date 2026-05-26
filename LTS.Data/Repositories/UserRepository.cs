using LTS.Common.Interfaces;
using LTS.Common.Models;
using Microsoft.Data.Sqlite;

namespace LTS.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connection = "Data Source=lts.db";

        public void Add(User user)
        {
            using var conn = new SqliteConnection(_connection);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText =
            @"INSERT INTO Users (Username, Password, Role, IsActive)
              VALUES ($username, $password, $role, $active)";

            cmd.Parameters.AddWithValue("$username", user.Username);
            cmd.Parameters.AddWithValue("$password", user.Password);
            cmd.Parameters.AddWithValue("$role", user.Role);
            cmd.Parameters.AddWithValue("$active", user.IsActive ? 1 : 0);

            cmd.ExecuteNonQuery();
        }

        public User GetByUsername(string username)
        {
            using var conn = new SqliteConnection(_connection);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Username = $username";
            cmd.Parameters.AddWithValue("$username", username);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User // db row converted into user object-mapping
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Role = reader.GetString(3),
                    IsActive = reader.GetInt32(4) == 1
                };
            }

            return null;
        }
    }
}