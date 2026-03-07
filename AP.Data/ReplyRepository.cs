using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AP.Models;

namespace AP.Data
{
    public class ReplyRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Reply> GetRepliesByThread(int threadId)
        {
            var list = new List<Reply>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ReplyId, ThreadId, UserId, Message, CreatedAt
                FROM Replies
                WHERE ThreadId = @threadId
                ORDER BY CreatedAt ASC", cn))
            {
                cmd.Parameters.AddWithValue("@threadId", threadId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Reply
                        {
                            ReplyId = Convert.ToInt32(dr["ReplyId"]),
                            ThreadId = Convert.ToInt32(dr["ThreadId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            Message = dr["Message"].ToString(),
                            CreatedAt = Convert.ToDateTime(dr["CreatedAt"])
                        });
                    }
                }
            }

            return list;
        }

        public void CreateReply(Reply model)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Replies(ThreadId, UserId, Message, CreatedAt)
                VALUES(@ThreadId, @UserId, @Message, @CreatedAt)", cn))
            {
                cmd.Parameters.AddWithValue("@ThreadId", model.ThreadId);
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@Message", model.Message);
                cmd.Parameters.AddWithValue("@CreatedAt", model.CreatedAt);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}