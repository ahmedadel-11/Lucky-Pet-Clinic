using Lucky_Pet_Clinic2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lucky_Pet_Clinic2.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<Pets>> GetPetsWithClientsAsync();
        Task RemoveByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task<IEnumerable<Examination>> GetAllExaminationAsync();
        Task<IEnumerable<Surgery>> GetAllSurgeryAsync();
        Task<IEnumerable<Vaccination>> GetAllVaccinationAsync();
    }
}
