using System.Configuration;
using System.Data.SqlClient;

namespace AP.Data
{
    public class DataProvider
    {
        private readonly string _cnn;

        public DataProvider()
        {
            _cnn = ConfigurationManager.ConnectionStrings["ForoUConnection"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_cnn);
        }
    }
}