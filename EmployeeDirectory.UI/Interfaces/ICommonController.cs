using EmployeeDirectory.Models;

namespace EmployeeDirectory.UI.Interfaces
{
    public interface ICommonController
    {
        ServiceResult<TTarget> Map<TSource, TTarget>(TSource source) where TTarget : new();
    }
}