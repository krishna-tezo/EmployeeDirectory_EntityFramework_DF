using EmployeeDirectory.Data.Data.Services;
using EmployeeDirectory.Models.SummaryModels;
namespace EmployeeDirectory.Data.Services
{
    public class RoleDataService : IRoleDataService
    {
        private ICommonDataService commonDataServices;
        public RoleDataService(ICommonDataService commonDataServices)
        {
            this.commonDataServices = commonDataServices;
        }
        
        public List<RoleSummary> GetRolesSummary()
        {
            string query = "SELECT R.Id,R.Name,R.DepartmentId,D.Name as Department, R.LocationId, L.Name as Location, " +
                "R.Description FROM Role R " +
                "JOIN Department D ON D.Id = R.DepartmentId " +
                "JOIN Location L ON L.Id = R.LocationId";

            return commonDataServices.GetAll(query, commonDataServices.MapObject<RoleSummary>);
        }

        public RoleSummary GetRoleSummaryById(string id)
        {
            string query = "SELECT R.Id,R.Name,R.DepartmentId,D.Name as Department, R.LocationId, L.Name as Location, " +
                "R.Description FROM Role R " +
                "JOIN Department D ON D.Id = R.DepartmentId " +
                "JOIN Location L ON L.Id = R.LocationId WHERE Id = @Id";

            return commonDataServices.Get(query,id, commonDataServices.MapObject<RoleSummary>);
        }
    }
}