using AP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AP.Data
{
    public class SubmissionRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Submission> GetByAssignment(int assignmentId)
        {
            var list = new List<Submission>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT s.SubmissionId, s.AssignmentId, s.StudentId, s.FileUrl, s.Comment, s.SubmittedAt,
                       u.FullName AS StudentName
                FROM Submissions s
                INNER JOIN Users u ON s.StudentId = u.UserId
                WHERE s.AssignmentId = @AssignmentId
                ORDER BY s.SubmittedAt DESC", cn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapRow(dr));
                }
            }

            return list;
        }

        public Submission GetByStudentAndAssignment(int studentId, int assignmentId)
        {
            Submission sub = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT s.SubmissionId, s.AssignmentId, s.StudentId, s.FileUrl, s.Comment, s.SubmittedAt,
                       u.FullName AS StudentName
                FROM Submissions s
                INNER JOIN Users u ON s.StudentId = u.UserId
                WHERE s.StudentId = @StudentId AND s.AssignmentId = @AssignmentId", cn))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        sub = MapRow(dr);
                }
            }

            return sub;
        }

        public void Create(Submission s)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Submissions (AssignmentId, StudentId, FileUrl, Comment, SubmittedAt)
                VALUES (@AssignmentId, @StudentId, @FileUrl, @Comment, GETDATE())", cn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", s.AssignmentId);
                cmd.Parameters.AddWithValue("@StudentId", s.StudentId);
                cmd.Parameters.AddWithValue("@FileUrl",
                    string.IsNullOrWhiteSpace(s.FileUrl) ? (object)DBNull.Value : s.FileUrl);
                cmd.Parameters.AddWithValue("@Comment",
                    string.IsNullOrWhiteSpace(s.Comment) ? (object)DBNull.Value : s.Comment);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static Submission MapRow(SqlDataReader dr)
        {
            return new Submission
            {
                SubmissionId = Convert.ToInt32(dr["SubmissionId"]),
                AssignmentId = Convert.ToInt32(dr["AssignmentId"]),
                StudentId    = Convert.ToInt32(dr["StudentId"]),
                FileUrl      = dr["FileUrl"] == DBNull.Value ? null : dr["FileUrl"].ToString(),
                Comment      = dr["Comment"] == DBNull.Value ? null : dr["Comment"].ToString(),
                SubmittedAt  = Convert.ToDateTime(dr["SubmittedAt"]),
                StudentName  = dr["StudentName"].ToString()
            };
        }
    }
}
