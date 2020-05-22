using Microsoft.EntityFrameworkCore;
using Resilience.Zeus.Domain.Interfaces;
using Resilience.Zeus.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resilience.Zeus.Infra.Data.Repository
{
	public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
	{
		protected readonly ZeusContext _context;

		protected readonly DbSet<TEntity> _dbSet;

		public Repository(ZeusContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity>();
		}

		public virtual void Add(TEntity obj)
		{
			_dbSet.Add(obj);
		}

		public virtual ValueTask<TEntity> GetByIdAsync(object id)
		{
			return _dbSet.FindAsync(id);
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return _dbSet;
		}

		public virtual void Update(TEntity obj)
		{
			_dbSet.Update(obj);
		}

		public virtual void Remove(Guid id)
		{
			_dbSet.Remove(_dbSet.Find(id));
		}

		public virtual IQueryable<TEntity> GetByPage<TKey>(int pageSize, int pageIndex, out int total,
			Func<TEntity, bool> whereLambda, bool isAsc, Func<TEntity, TKey> orderByLambda)
		{
			var tempData = _dbSet.Where(whereLambda);

			total = tempData.Count();

			if (isAsc)
			{
				tempData = tempData.OrderBy(orderByLambda).
					  Skip(pageSize * (pageIndex - 1)).
					  Take(pageSize).AsQueryable();
			}
			else
			{
				tempData = tempData.OrderByDescending(orderByLambda).
					 Skip(pageSize * (pageIndex - 1)).
					 Take(pageSize).AsQueryable();
			}
			return tempData.AsQueryable();
		}

		public void Dispose()
		{
			_context.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
