using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AP.Models;

namespace AP.Data
{
    // DP: Repository - toda la logica de acceso a la tabla Threads queda aqui
    // asi el resto del proyecto no tiene que saber nada de SQL
    public class ThreadRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Thread> GetThreads()
        {
            var list = new List<Thread>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT t.ThreadId, t.CategoryId, t.UserId, t.Title, t.Message, t.CreatedAt, t.IsClosed,
                       u.FullName AS UserName
                FROM Threads t
                INNER JOIN Users u ON u.UserId = t.UserId
                ORDER BY t.CreatedAt DESC", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(MapThread(dr));
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
                SELECT t.ThreadId, t.CategoryId, t.UserId, t.Title, t.Message, t.CreatedAt, t.IsClosed,
                       u.FullName AS UserName
                FROM Threads t
                INNER JOIN Users u ON u.UserId = t.UserId
                WHERE t.CategoryId = @CategoryId
                ORDER BY t.CreatedAt DESC", cn))
            {
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(MapThread(dr));
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
                SELECT t.ThreadId, t.CategoryId, t.UserId, t.Title, t.Message, t.CreatedAt, t.IsClosed,
                       u.FullName AS UserName
                FROM Threads t
                INNER JOIN Users u ON u.UserId = t.UserId
                WHERE t.ThreadId = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        t = MapThread(dr);
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
                cmd.Parameters.AddWithValue("@Title", model.Title.Trim());
                cmd.Parameters.AddWithValue("@Message", model.Message.Trim());
                cmd.Parameters.AddWithValue("@CreatedAt", model.CreatedAt);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void SetClosed(int threadId, bool closed)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Threads SET IsClosed = @closed WHERE ThreadId = @id", cn))
            {
                cmd.Parameters.AddWithValue("@closed", closed ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", threadId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // DP: algo parecido a Factory Method - mapeamos el DataReader una sola vez
        // para no repetir ese codigo en cada metodo que hace un SELECT
        private Thread MapThread(SqlDataReader dr)
        {
            return new Thread
            {
                ThreadId   = Convert.ToInt32(dr["ThreadId"]),
                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                UserId     = Convert.ToInt32(dr["UserId"]),
                Title      = dr["Title"].ToString(),
                Message    = dr["Message"].ToString(),
                CreatedAt  = Convert.ToDateTime(dr["CreatedAt"]),
                UserName   = dr["UserName"].ToString(),
                IsClosed   = Convert.ToBoolean(dr["IsClosed"])
            };
        }
    }
}
