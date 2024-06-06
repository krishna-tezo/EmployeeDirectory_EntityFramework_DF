using AutoMapper;
using SM = EmployeeDirectory.Models.Models;

namespace EmployeeDirectory.Data.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            
            CreateMap<Employee, SM.Employee>().ReverseMap();
            CreateMap<Role, SM.Role>().ReverseMap();
            CreateMap<Project, SM.Project>().ReverseMap();
            CreateMap<Department, SM.Department>().ReverseMap();
            CreateMap<Location, SM.Location>().ReverseMap();
            CreateMap<Manager, SM.Manager>().ReverseMap();
        }
    }
}
