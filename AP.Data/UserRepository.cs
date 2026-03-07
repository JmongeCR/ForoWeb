using AP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AP.Data
{
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
                        list.Add(new User
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            FullName = dr["FullName"].ToString(),
                            Email = dr["Email"].ToString(),
                            PasswordHash = dr["PasswordHash"].ToString(),
                            Role = dr["Role"].ToString(),
                            PhotoUrl = dr["PhotoUrl"] == DBNull.Value ? null : dr["PhotoUrl"].ToString(),
                            IsActive = Convert.ToBoolean(dr["IsActive"])
                        });
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
                cmd.Parameters.AddWithValue("@FullName", model.FullName);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                cmd.Parameters.AddWithValue("@Role", model.Role);
                cmd.Parameters.AddWithValue("@PhotoUrl",
                    string.IsNullOrWhiteSpace(model.PhotoUrl) ? (object)DBNull.Value : model.PhotoUrl);
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
                WHERE Email = @Email
                  AND PasswordHash = @PasswordHash
                  AND IsActive = 1", cn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PasswordHash", password);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        user = new User
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
                        user = new User
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

            return user;
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
                cmd.Parameters.AddWithValue("@FullName", model.FullName);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                cmd.Parameters.AddWithValue("@Role", model.Role);
                cmd.Parameters.AddWithValue("@PhotoUrl",
                    string.IsNullOrWhiteSpace(model.PhotoUrl) ? (object)DBNull.Value : model.PhotoUrl);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}