using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AP.Models;

namespace AP.Data
{
    // Accede directamente a la tabla Notifications con ADO.NET
    // igual que todos los otros repositorios del proyecto
    public class NotificationRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Notification> GetByUser(int userId)
        {
            var list = new List<Notification>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT NotificationId, UserId, Message, Link, IsRead, CreatedAt
                FROM Notifications
                WHERE UserId = @UserId
                ORDER BY CreatedAt DESC", cn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapRow(dr));
                }
            }

            return list;
        }

        public int GetUnreadCount(int userId)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT COUNT(1) FROM Notifications
                WHERE UserId = @UserId AND IsRead = 0", cn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void Create(Notification n)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Notifications (UserId, Message, Link, IsRead, CreatedAt)
                VALUES (@UserId, @Message, @Link, 0, GETDATE())", cn))
            {
                cmd.Parameters.AddWithValue("@UserId", n.UserId);
                cmd.Parameters.AddWithValue("@Message", n.Message);
                cmd.Parameters.AddWithValue("@Link", (object)n.Link ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void MarkAllRead(int userId)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE Notifications SET IsRead = 1
                WHERE UserId = @UserId AND IsRead = 0", cn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static Notification MapRow(SqlDataReader dr)
        {
            return new Notification
            {
                NotificationId = Convert.ToInt32(dr["NotificationId"]),
                UserId         = Convert.ToInt32(dr["UserId"]),
                Message        = dr["Message"].ToString(),
                Link           = dr["Link"] == DBNull.Value ? null : dr["Link"].ToString(),
                IsRead         = Convert.ToBoolean(dr["IsRead"]),
                CreatedAt      = Convert.ToDateTime(dr["CreatedAt"])
            };
        }
    }
}
