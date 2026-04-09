using AP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AP.Data
{
    public class AssignmentRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Assignment> GetByProfessor(int professorId)
        {
            var list = new List<Assignment>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT a.AssignmentId, a.ProfessorId, a.ClassId, a.Title, a.Description, a.DueDate, a.CreatedAt,
                       cl.Name AS ClassName
                FROM Assignments a
                INNER JOIN Clases cl ON a.ClassId = cl.ClaseId
                WHERE cl.ProfessorId = @ProfessorId
                ORDER BY a.CreatedAt DESC", cn))
            {
                cmd.Parameters.AddWithValue("@ProfessorId", professorId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapRow(dr));
                }
            }

            return list;
        }

        public List<Assignment> GetByStudent(int studentId)
        {
            var list = new List<Assignment>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT a.AssignmentId, a.ProfessorId, a.ClassId, a.Title, a.Description, a.DueDate, a.CreatedAt,
                       cl.Name AS ClassName
                FROM Assignments a
                INNER JOIN Clases cl ON a.ClassId = cl.ClaseId
                INNER JOIN ClassStudents cs ON cl.ClaseId = cs.ClaseId
                WHERE cs.StudentId = @StudentId
                ORDER BY a.DueDate ASC", cn))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapRow(dr));
                }
            }

            return list;
        }

        public Assignment GetById(int id)
        {
            Assignment a = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT a.AssignmentId, a.ProfessorId, a.ClassId, a.Title, a.Description, a.DueDate, a.CreatedAt,
                       cl.Name AS ClassName
                FROM Assignments a
                INNER JOIN Clases cl ON a.ClassId = cl.ClaseId
                WHERE a.AssignmentId = @AssignmentId", cn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        a = MapRow(dr);
                }
            }

            return a;
        }

        public int Create(Assignment a)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Assignments (ProfessorId, ClassId, Title, Description, DueDate, CreatedAt)
                VALUES (@ProfessorId, @ClassId, @Title, @Description, @DueDate, GETDATE());
                SELECT SCOPE_IDENTITY();", cn))
            {
                cmd.Parameters.AddWithValue("@ProfessorId", a.ProfessorId);
                cmd.Parameters.AddWithValue("@ClassId", a.ClassId);
                cmd.Parameters.AddWithValue("@Title", a.Title);
                cmd.Parameters.AddWithValue("@Description", a.Description);
                cmd.Parameters.AddWithValue("@DueDate", a.DueDate);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<User> GetAssignedStudents(int assignmentId)
        {
            var list = new List<User>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT u.UserId, u.FullName, u.Email, u.Role, u.PhotoUrl, u.IsActive
                FROM Users u
                INNER JOIN ClassStudents cs ON u.UserId = cs.StudentId
                INNER JOIN Assignments a ON cs.ClaseId = a.ClassId
                WHERE a.AssignmentId = @AssignmentId
                ORDER BY u.FullName", cn))
            {
                cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new User
                        {
                            UserId   = Convert.ToInt32(dr["UserId"]),
                            FullName = dr["FullName"].ToString(),
                            Email    = dr["Email"].ToString(),
                            Role     = dr["Role"].ToString(),
                            PhotoUrl = dr["PhotoUrl"] == DBNull.Value ? null : dr["PhotoUrl"].ToString(),
                            IsActive = Convert.ToBoolean(dr["IsActive"])
                        });
                    }
                }
            }

            return list;
        }

        public bool IsStudentAssigned(int assignmentId, int studentId)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT COUNT(1)
                FROM ClassStudents cs
                INNER JOIN Assignments a ON cs.ClaseId = a.ClassId
                WHERE a.AssignmentId = @Aid AND cs.StudentId = @Sid", cn))
            {
                cmd.Parameters.AddWithValue("@Aid", assignmentId);
                cmd.Parameters.AddWithValue("@Sid", studentId);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private static Assignment MapRow(SqlDataReader dr)
        {
            return new Assignment
            {
                AssignmentId = Convert.ToInt32(dr["AssignmentId"]),
                ProfessorId  = Convert.ToInt32(dr["ProfessorId"]),
                ClassId      = Convert.ToInt32(dr["ClassId"]),
                Title        = dr["Title"].ToString(),
                Description  = dr["Description"].ToString(),
                ClassName    = dr["ClassName"].ToString(),
                DueDate      = Convert.ToDateTime(dr["DueDate"]),
                CreatedAt    = Convert.ToDateTime(dr["CreatedAt"])
            };
        }
    }
}
