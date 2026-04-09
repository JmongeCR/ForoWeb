using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AP.Models;

namespace AP.Data
{
    public class AssignmentRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Assignment> GetAssignments()
        {
            var list = new List<Assignment>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT a.AssignmentId, a.CategoryId, a.TeacherId, a.Title, a.Description, a.DueDate,
                       c.Name AS CategoryName,
                       u.FullName AS TeacherName
                FROM Assignments a
                INNER JOIN Categories c ON c.CategoryId = a.CategoryId
                INNER JOIN Users u ON u.UserId = a.TeacherId
                ORDER BY a.DueDate ASC, a.AssignmentId DESC", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(MapAssignment(dr));
                    }
                }
            }

            return list;
        }

        public Assignment GetAssignmentById(int id)
        {
            Assignment assignment = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT a.AssignmentId, a.CategoryId, a.TeacherId, a.Title, a.Description, a.DueDate,
                       c.Name AS CategoryName,
                       u.FullName AS TeacherName
                FROM Assignments a
                INNER JOIN Categories c ON c.CategoryId = a.CategoryId
                INNER JOIN Users u ON u.UserId = a.TeacherId
                WHERE a.AssignmentId = @AssignmentId", cn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        assignment = MapAssignment(dr);
                    }
                }
            }

            return assignment;
        }

        public void CreateAssignment(Assignment model)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Assignments (CategoryId, TeacherId, Title, Description, DueDate)
                VALUES (@CategoryId, @TeacherId, @Title, @Description, @DueDate)", cn))
            {
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@TeacherId", model.TeacherId);
                cmd.Parameters.AddWithValue("@Title", model.Title.Trim());
                cmd.Parameters.AddWithValue("@Description", model.Description.Trim());
                cmd.Parameters.AddWithValue("@DueDate", model.DueDate.Date);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private Assignment MapAssignment(SqlDataReader dr)
        {
            return new Assignment
            {
                AssignmentId = Convert.ToInt32(dr["AssignmentId"]),
                CategoryId = Convert.ToInt32(dr["CategoryId"]),
                TeacherId = Convert.ToInt32(dr["TeacherId"]),
                Title = dr["Title"].ToString(),
                Description = dr["Description"].ToString(),
                DueDate = Convert.ToDateTime(dr["DueDate"]),
                CategoryName = dr["CategoryName"].ToString(),
                TeacherName = dr["TeacherName"].ToString()
            };
        }
    }
}
