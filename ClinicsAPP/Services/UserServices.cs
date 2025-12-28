using ClinicsAPP.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using ClinicsAPP.Models;

namespace ClinicsAPP.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Note: In a real application, use password hashing (e.g., BCrypt or Identity PasswordHasher)
                // This example uses a simple query for demonstration
                string query = "SELECT Id, Email, FullName FROM Users WHERE Email = @Email AND PasswordHash = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password); // Should be hashed password

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Email = reader["Email"].ToString(),
                                FullName = reader["FullName"].ToString()
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
