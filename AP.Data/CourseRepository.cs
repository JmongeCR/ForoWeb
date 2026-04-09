using AP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AP.Data
{
    public class CourseRepository
    {
        private readonly DataProvider _db = new DataProvider();

        public List<Course> GetAll()
        {
            var list = new List<Course>();

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT CourseId, Name, Description, CreatedAt FROM Courses ORDER BY Name", cn))
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

        public Course GetById(int id)
        {
            Course course = null;

            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT CourseId, Name, Description, CreatedAt FROM Courses WHERE CourseId = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        course = MapRow(dr);
                }
            }

            return course;
        }

        public void Create(Course c)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO Courses (Name, Description, CreatedAt) VALUES (@Name, @Desc, GETDATE())", cn))
            {
                cmd.Parameters.AddWithValue("@Name", c.Name);
                cmd.Parameters.AddWithValue("@Desc", (object)c.Description ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Course c)
        {
            using (SqlConnection cn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Courses SET Name = @Name, Description = @Desc WHERE CourseId = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Name", c.Name);
                cmd.Parameters.AddWithValue("@Desc", (object)c.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", c.CourseId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static Course MapRow(SqlDataReader dr)
        {
            return new Course
            {
                CourseId    = Convert.ToInt32(dr["CourseId"]),
                Name        = dr["Name"].ToString(),
                Description = dr["Description"] == DBNull.Value ? null : dr["Description"].ToString(),
                CreatedAt   = Convert.ToDateTime(dr["CreatedAt"])
            };
        }
    }
}
