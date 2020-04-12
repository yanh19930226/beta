﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resilience.Zeus.Domain.Interfaces
{
	public interface IRepository<TEntity> : IDisposable where TEntity : class
	{
		void Add(TEntity obj);

		ValueTask<TEntity> GetByIdAsync(object id);

		IQueryable<TEntity> GetAll();

		void Update(TEntity obj);

		void Remove(Guid id);
	}
}
