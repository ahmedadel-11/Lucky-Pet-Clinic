using Lucky_Pet_Clinic2.Helpers;
using Lucky_Pet_Clinic2.Models;
using System.Collections.Generic;
using System.Linq;

public interface IYourEntityRepository
{
    IEnumerable<YourEntity> GetAll();
    YourEntity GetById(int id);
    void Add(YourEntity entity);
    void Update(YourEntity entity);
    void Delete(int id);
}

public class YourEntityRepository : IYourEntityRepository
{
    private readonly AppDbContext _context;

    public YourEntityRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<YourEntity> GetAll()
    {
        return _context.YourEntities.ToList();
    }

    public YourEntity GetById(int id)
    {
        return _context.YourEntities.Find(id);
    }

    public void Add(YourEntity entity)
    {
        _context.YourEntities.Add(entity);
        _context.SaveChanges();
    }

    public void Update(YourEntity entity)
    {
        _context.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.YourEntities.Find(id);
        if (entity != null)
        {
            _context.YourEntities.Remove(entity);
            _context.SaveChanges();
        }
    }
}
