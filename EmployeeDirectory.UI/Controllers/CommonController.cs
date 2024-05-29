using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models;
using EmployeeDirectory.UI.Interfaces;
using EmployeeDirectory.ViewModel;

namespace EmployeeDirectory.UI.Controllers
{
    public class CommonController : ICommonController
    {
        public ServiceResult<TTarget> Map<TSource, TTarget>(TSource source) where TTarget : new()
        {
            TTarget target = new TTarget();
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            var targetProperties = targetType.GetProperties();

            foreach (var targetProp in targetProperties)
            {
                if (targetProp.Name == "Name" && typeof(TSource) == typeof(EmployeeSummary) && typeof(TTarget) == typeof(EmployeeView))
                {
                    var firstName = sourceType.GetProperty("FirstName")?.GetValue(source)?.ToString();
                    var lastName = sourceType.GetProperty("LastName")?.GetValue(source)?.ToString();
                    targetProp.SetValue(target, $"{firstName} {lastName}");
                    continue;
                }

                var sourceProp = sourceType.GetProperty(targetProp.Name);
                if (sourceProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                }
            }

            if (target != null)
            {
                return ServiceResult<TTarget>.Success(target);
            }
            else
            {
                return ServiceResult<TTarget>.Fail("Error Occurred while mapping");
            }
        }
    }
}
