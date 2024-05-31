using EmployeeDirectory.Models.Models;
using System.ComponentModel;
using System.Linq.Expressions;


namespace EmployeeDirectory.Data.Services
{
    public class CommonDataService : ICommonDataService
    {
        private AppDBContext context;
        public CommonDataService(AppDBContext context)
        {
            this.context = context;
        }

        //Get All
        public List<T> GetAll<T>() where T : class
        {

            List<T> collection = new List<T>();
            collection = context.Set<T>().ToList();
            return collection;

        }

        //Get By Id
        public T Get<T>(string id) where T : class
        {
            return context.Set<T>().Find(id);
        }

        //Insert
        public int InsertOrUpdate<T>(T obj)
        {
            context.Add(obj);
            int changesSaved = context.SaveChanges();

            return changesSaved;
        }

        //Delete
        public int DeleteById<T>(string id) where T : class
        {
            int changesSaved = 0;
            T obj = context.Set<T>().Find(id);

            context.Remove(obj);
            changesSaved = context.SaveChanges();

            return changesSaved;
        }

        public T GetLast<T>() where T : class
        {
            var type = typeof(T);
            var idProperty = type.GetProperty("Id");

            if (idProperty == null)
            {
                throw new InvalidOperationException($"The type {type.Name} does not have an 'id' property.");
            }

            var parameter = Expression.Parameter(type, "x");
            var propertyExpression = Expression.Property(parameter, idProperty);
            var lambda = Expression.Lambda(propertyExpression, parameter);

            var orderByDescendingMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2)
                .MakeGenericMethod(type, idProperty.PropertyType);

            var orderedQuery = (IQueryable<T>)orderByDescendingMethod.Invoke(null, new object[] { context.Set<T>(), lambda });

            return orderedQuery.FirstOrDefault();
        }


    }
}
