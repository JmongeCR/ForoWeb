using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AP.Models;

namespace AP.Data
{
    public class ThreadRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Thread> GetThreads()
        {
            var list = new List<Thread>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ThreadId, CategoryId, UserId, Title, Message, CreatedAt
                FROM Threads
                ORDER BY CreatedAt DESC", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Thread
                        {
                            ThreadId = Convert.ToInt32(dr["ThreadId"]),
                            CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            Title = dr["Title"].ToString(),
                            Message = dr["Message"].ToString(),
                            CreatedAt = Convert.ToDateTime(dr["CreatedAt"])
                        });
                    }
                }
            }

            return list;
        }
        public List<Thread> GetThreadsByCategory(int categoryId)
        {
            var list = new List<Thread>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT ThreadId, CategoryId, UserId, Title, Message, CreatedAt
        FROM Threads
        WHERE CategoryId = @CategoryId
        ORDER BY CreatedAt DESC", cn))
            {
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Thread
                        {
                            ThreadId = Convert.ToInt32(dr["ThreadId"]),
                            CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            Title = dr["Title"].ToString(),
                            Message = dr["Message"].ToString(),
                            CreatedAt = Convert.ToDateTime(dr["CreatedAt"])
                        });
                    }
                }
            }

            return list;
        }
        public Thread GetThread(int id)
        {
            Thread t = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ThreadId, CategoryId, UserId, Title, Message, CreatedAt
                FROM Threads
                WHERE ThreadId = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        t = new Thread
                        {
                            ThreadId = Convert.ToInt32(dr["ThreadId"]),
                            CategoryId = Convert.ToInt32(dr["CategoryId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            Title = dr["Title"].ToString(),
                            Message = dr["Message"].ToString(),
                            CreatedAt = Convert.ToDateTime(dr["CreatedAt"])
                        };
                    }
                }
            }

            return t;
        }

        public void CreateThread(Thread model)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Threads(CategoryId, UserId, Title, Message, CreatedAt)
                VALUES(@CategoryId, @UserId, @Title, @Message, @CreatedAt)", cn))
            {
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@Message", model.Message);
                cmd.Parameters.AddWithValue("@CreatedAt", model.CreatedAt);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}