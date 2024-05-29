

using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.Data
{
    public interface IDbConnection
    {
        SqlConnection GetConnection();
    }
}