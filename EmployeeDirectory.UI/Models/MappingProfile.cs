using AutoMapper;
using EmployeeDirectory.Data.Models;
using EmployeeDirectory.Model;

namespace EmployeeDirectory.UI.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeView>().ReverseMap();
            CreateMap<Role, RoleView>().ReverseMap();
        }
    }
}
