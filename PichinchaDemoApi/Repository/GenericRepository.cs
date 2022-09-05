using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using PichinchaDemoApi.Models;
using PichinchaDemoApi.Data;

namespace PichinchaDemoApi.Repository;

public class GenericRepository<T> where T : class
{
    internal DataContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async virtual Task<List<T>> ObtenerTodos(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public async virtual Task<T> Obtener(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async virtual Task Agregar(T entidad)
    {
        await _dbSet.AddAsync(entidad);
    }

    public async virtual Task Eliminar(object id)
    {
        T entidad = await _dbSet.FindAsync(id);
        Eliminar(entidad);
    }

    public virtual void Eliminar(T entidad)
    {
        if (_context.Entry(entidad).State == EntityState.Detached)
        {
            _dbSet.Attach(entidad);
        }
        _dbSet.Remove(entidad);
    }
}