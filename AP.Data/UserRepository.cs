using AP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AP.Data
{
    // DP: Repository - concentramos todo lo de la tabla Users en un solo lugar
    // SOLID: SRP - esta clase tiene una sola razon para cambiar: si cambia algo en los datos de usuarios
    public class UserRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<User> GetUsers()
        {
            var list = new List<User>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT UserId, FullName, Email, PasswordHash, Role, PhotoUrl, IsActive
                FROM Users
                ORDER BY UserId DESC", cn))
            {
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(MapUser(dr));
                    }
                }
            }

            return list;
        }

        public void CreateUser(User model)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Users (FullName, Email, PasswordHash, Role, PhotoUrl, IsActive)
                VALUES (@FullName, @Email, @PasswordHash, @Role, @PhotoUrl, @IsActive)", cn))
            {
                cmd.Parameters.AddWithValue("@FullName", model.FullName.Trim());
                cmd.Parameters.AddWithValue("@Email", model.Email.Trim());
                cmd.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                cmd.Parameters.AddWithValue("@Role", model.Role);
                cmd.Parameters.AddWithValue("@PhotoUrl",
                    string.IsNullOrWhiteSpace(model.PhotoUrl) ? (object)DBNull.Value : model.PhotoUrl.Trim());
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public User GetByEmailAndPassword(string email, string password)
        {
            User user = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT UserId, FullName, Email, PasswordHash, Role, PhotoUrl, IsActive
                FROM Users
                WHERE LOWER(LTRIM(RTRIM(Email))) = LOWER(LTRIM(RTRIM(@Email)))
                  AND PasswordHash = @PasswordHash
                  AND IsActive = 1", cn))
            {
                cmd.Parameters.AddWithValue("@Email", email ?? string.Empty);
                cmd.Parameters.AddWithValue("@PasswordHash", password ?? string.Empty);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        user = MapUser(dr);
                    }
                }
            }

            return user;
        }

        public User GetUserById(int id)
        {
            User user = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT UserId, FullName, Email, PasswordHash, Role, PhotoUrl, IsActive
                FROM Users
                WHERE UserId = @UserId", cn))
            {
                cmd.Parameters.AddWithValue("@UserId", id);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        user = MapUser(dr);
                    }
                }
            }

            return user;
        }

        public bool EmailExists(string email, int excludeUserId = 0)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT COUNT(1)
                FROM Users
                WHERE LOWER(LTRIM(RTRIM(Email))) = LOWER(LTRIM(RTRIM(@Email)))
                  AND UserId <> @UserId", cn))
            {
                cmd.Parameters.AddWithValue("@Email", email ?? string.Empty);
                cmd.Parameters.AddWithValue("@UserId", excludeUserId);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void UpdateUser(User model)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE Users
                SET FullName = @FullName,
                    Email = @Email,
                    PasswordHash = @PasswordHash,
                    Role = @Role,
                    PhotoUrl = @PhotoUrl,
                    IsActive = @IsActive
                WHERE UserId = @UserId", cn))
            {
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@FullName", model.FullName.Trim());
                cmd.Parameters.AddWithValue("@Email", model.Email.Trim());
                cmd.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                cmd.Parameters.AddWithValue("@Role", model.Role);
                cmd.Parameters.AddWithValue("@PhotoUrl",
                    string.IsNullOrWhiteSpace(model.PhotoUrl) ? (object)DBNull.Value : model.PhotoUrl.Trim());
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private User MapUser(SqlDataReader dr)
        {
            return new User
            {
                UserId = Convert.ToInt32(dr["UserId"]),
                FullName = dr["FullName"].ToString(),
                Email = dr["Email"].ToString(),
                PasswordHash = dr["PasswordHash"].ToString(),
                Role = dr["Role"].ToString(),
                PhotoUrl = dr["PhotoUrl"] == DBNull.Value ? null : dr["PhotoUrl"].ToString(),
                IsActive = Convert.ToBoolean(dr["IsActive"])
            };
        }
    }
}
