using AP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AP.Data
{
    public class ClaseRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Clase> GetAll()
        {
            var list = new List<Clase>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT cl.ClaseId, cl.CourseId, cl.ProfessorId, cl.Name, cl.CreatedAt,
                       co.Name AS CourseName,
                       u.FullName AS ProfessorName
                FROM Clases cl
                INNER JOIN Courses co ON cl.CourseId = co.CourseId
                INNER JOIN Users u ON cl.ProfessorId = u.UserId
                ORDER BY co.Name, cl.Name", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapRow(dr));
                }
            }

            return list;
        }

        public Clase GetById(int id)
        {
            Clase clase = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT cl.ClaseId, cl.CourseId, cl.ProfessorId, cl.Name, cl.CreatedAt,
                       co.Name AS CourseName,
                       u.FullName AS ProfessorName
                FROM Clases cl
                INNER JOIN Courses co ON cl.CourseId = co.CourseId
                INNER JOIN Users u ON cl.ProfessorId = u.UserId
                WHERE cl.ClaseId = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        clase = MapRow(dr);
                }
            }

            return clase;
        }

        public List<Clase> GetByProfessor(int professorId)
        {
            var list = new List<Clase>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT cl.ClaseId, cl.CourseId, cl.ProfessorId, cl.Name, cl.CreatedAt,
                       co.Name AS CourseName,
                       u.FullName AS ProfessorName
                FROM Clases cl
                INNER JOIN Courses co ON cl.CourseId = co.CourseId
                INNER JOIN Users u ON cl.ProfessorId = u.UserId
                WHERE cl.ProfessorId = @ProfessorId
                ORDER BY cl.Name", cn))
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

        public List<Clase> GetByStudent(int studentId)
        {
            var list = new List<Clase>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT cl.ClaseId, cl.CourseId, cl.ProfessorId, cl.Name, cl.CreatedAt,
                       co.Name AS CourseName,
                       u.FullName AS ProfessorName
                FROM Clases cl
                INNER JOIN Courses co ON cl.CourseId = co.CourseId
                INNER JOIN Users u ON cl.ProfessorId = u.UserId
                INNER JOIN ClassStudents cs ON cl.ClaseId = cs.ClaseId
                WHERE cs.StudentId = @StudentId
                ORDER BY cl.Name", cn))
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

        public void Create(Clase c)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Clases (CourseId, ProfessorId, Name, CreatedAt)
                VALUES (@CourseId, @ProfessorId, @Name, GETDATE())", cn))
            {
                cmd.Parameters.AddWithValue("@CourseId", c.CourseId);
                cmd.Parameters.AddWithValue("@ProfessorId", c.ProfessorId);
                cmd.Parameters.AddWithValue("@Name", c.Name);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Clase c)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE Clases SET CourseId = @CourseId, ProfessorId = @ProfessorId, Name = @Name
                WHERE ClaseId = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@CourseId", c.CourseId);
                cmd.Parameters.AddWithValue("@ProfessorId", c.ProfessorId);
                cmd.Parameters.AddWithValue("@Name", c.Name);
                cmd.Parameters.AddWithValue("@Id", c.ClaseId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<User> GetStudents(int claseId)
        {
            var list = new List<User>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT u.UserId, u.FullName, u.Email, u.Role, u.PhotoUrl, u.IsActive
                FROM Users u
                INNER JOIN ClassStudents cs ON u.UserId = cs.StudentId
                WHERE cs.ClaseId = @ClaseId
                ORDER BY u.FullName", cn))
            {
                cmd.Parameters.AddWithValue("@ClaseId", claseId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapUser(dr));
                }
            }

            return list;
        }

        public List<User> GetAvailableStudents(int claseId)
        {
            var list = new List<User>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT u.UserId, u.FullName, u.Email, u.Role, u.PhotoUrl, u.IsActive
                FROM Users u
                WHERE u.Role = 'Estudiante' AND u.IsActive = 1
                AND u.UserId NOT IN (
                    SELECT StudentId FROM ClassStudents WHERE ClaseId = @ClaseId
                )
                ORDER BY u.FullName", cn))
            {
                cmd.Parameters.AddWithValue("@ClaseId", claseId);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(MapUser(dr));
                }
            }

            return list;
        }

        public void AddStudent(int claseId, int studentId)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                IF NOT EXISTS (SELECT 1 FROM ClassStudents WHERE ClaseId = @ClaseId AND StudentId = @StudentId)
                    INSERT INTO ClassStudents (ClaseId, StudentId) VALUES (@ClaseId, @StudentId)", cn))
            {
                cmd.Parameters.AddWithValue("@ClaseId", claseId);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveStudent(int claseId, int studentId)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM ClassStudents WHERE ClaseId = @ClaseId AND StudentId = @StudentId", cn))
            {
                cmd.Parameters.AddWithValue("@ClaseId", claseId);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsStudentInClase(int claseId, int studentId)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM ClassStudents WHERE ClaseId = @ClaseId AND StudentId = @StudentId", cn))
            {
                cmd.Parameters.AddWithValue("@ClaseId", claseId);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private static Clase MapRow(SqlDataReader dr)
        {
            return new Clase
            {
                ClaseId       = Convert.ToInt32(dr["ClaseId"]),
                CourseId      = Convert.ToInt32(dr["CourseId"]),
                ProfessorId   = Convert.ToInt32(dr["ProfessorId"]),
                Name          = dr["Name"].ToString(),
                CourseName    = dr["CourseName"].ToString(),
                ProfessorName = dr["ProfessorName"].ToString(),
                CreatedAt     = Convert.ToDateTime(dr["CreatedAt"])
            };
        }

        private static User MapUser(SqlDataReader dr)
        {
            return new User
            {
                UserId   = Convert.ToInt32(dr["UserId"]),
                FullName = dr["FullName"].ToString(),
                Email    = dr["Email"].ToString(),
                Role     = dr["Role"].ToString(),
                PhotoUrl = dr["PhotoUrl"] == DBNull.Value ? null : dr["PhotoUrl"].ToString(),
                IsActive = Convert.ToBoolean(dr["IsActive"])
            };
        }
    }
}
