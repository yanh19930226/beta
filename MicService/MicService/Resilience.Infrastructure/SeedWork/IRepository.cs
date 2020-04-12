﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resilience.Infrastructure.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<T> GetAsync(int id);

        T Add(T t);

        T Update(T t);
        IUnitOfWork UnitOfWork { get; }
    }
}
