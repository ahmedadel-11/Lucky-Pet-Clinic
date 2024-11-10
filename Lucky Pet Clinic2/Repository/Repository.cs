using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lucky_Pet_Clinic2.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Pets>> GetPetsWithClientsAsync()
        {
            return await _context.Pets
                .Include(p => p.Client) // Include the related Client entity
                .ToListAsync();
        }

        public async Task<IEnumerable<Examination>> GetAllExaminationAsync()
        {
            return await _context.Examinations
                .Include(e => e.Pet)
                .ThenInclude(p => p.Client)
                .ToListAsync();
        }
        public async Task<IEnumerable<Surgery>> GetAllSurgeryAsync()
        {
            return await _context.Surgeries
                .Include(e => e.Pet)
                .ThenInclude(p => p.Client)
                .ToListAsync();
        }
        public async Task<IEnumerable<Vaccination>> GetAllVaccinationAsync()
        {
            return await _context.Vaccinations
                .Include(e => e.Pet)
                .ThenInclude(p => p.Client)
                .ToListAsync();
        }

        // Repeat for Surgery and Vaccination models


    }
}
