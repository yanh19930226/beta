﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resilience.Zeus.Domain.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		Task<bool> CommitAsync();
	}
}
