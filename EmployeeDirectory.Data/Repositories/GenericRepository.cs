using EmployeeDirectory.Data.Interfaces;
using EmployeeDirectory.Data.Models;

namespace EmployeeDirectory.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext context;

        public GenericRepository(AppDBContext context)
        {
            this.context = context;
        }

        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T Get(string id)
        {
            return context.Set<T>().Find(id);
        }

        public int Insert(T obj)
        {
            context.Add(obj);
            return context.SaveChanges();
        }

        public int Update(T obj)
        {
           
            context.Update(obj);
            return context.SaveChanges();
        }

        public int DeleteById(string id)
        {
            T obj = context.Set<T>().Find(id);
            context.Remove(obj);
            return context.SaveChanges();
        }

        public T GetLast()
        {
            var type = typeof(T);
            var idProperty = type.GetProperty("Id");

            if (idProperty == null)
            {
                throw new InvalidOperationException($"The type {type.Name} does not have an 'id' property.");
            }
            else
            {
                T obj = context.Set<T>().ToList().Last();
                return obj;
            }
        }
    }
}