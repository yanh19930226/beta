using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Resilience.Zeus.Domain.Core.Events;
using Resilience.Zeus.Domain.Core.Models;
using Resilience.Zeus.Domain.Interfaces;
using Resilience.Zeus.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resilience.Zeus.Infra.Data.Uow
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly DbContext _context;

		private readonly IMediator _mediator;

		public UnitOfWork(ZeusContext context, IMediator mediator)
		{
			_context = context;
			_mediator = mediator;
		}

		public async Task<bool> CommitAsync()
		{
			bool isSuccess = await _context.SaveChangesAsync() > 0;
			if (isSuccess)
			{
				var domainEntities = _context.ChangeTracker
			   .Entries<Entity>()
			   .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

				var domainEvents = domainEntities
					.SelectMany(x => x.Entity.DomainEvents)
					.ToList();

				domainEntities.ToList()
					.ForEach(entity => entity.Entity.ClearDomainEvents());

				var tasks = domainEvents
					.Select(async (domainEvent) => {
						await _mediator.Publish(domainEvent);
					});

				await Task.WhenAll(tasks);
			}
			return isSuccess;
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
