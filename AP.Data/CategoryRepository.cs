using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using AP.Models;

namespace AP.Data
{
    public class CategoryRepository
    {
        private readonly string _cs;

        public CategoryRepository()
        {
            _cs = ConfigurationManager.ConnectionStrings["ForoUConnection"].ConnectionString;
        }

        public List<Category> GetAll()
        {
            var list = new List<Category>();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT CategoryId, Name, Description FROM dbo.Categories ORDER BY Name", cn))
            {
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new Category
                        {
                            CategoryId = (int)rd["CategoryId"],
                            Name = (string)rd["Name"],
                            Description = rd["Description"] as string
                        });
                    }
                }
            }

            return list;
        }
    }
}
