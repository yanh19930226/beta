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
				IEnumerable<EntityEntry<Entity>> source = _context.ChangeTracker.Entries<Entity>().Where((Func<EntityEntry<Entity>, bool>)((EntityEntry<Entity> x) => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()));
				List<Event> source2 = source.SelectMany((Func<EntityEntry<Entity>, IEnumerable<Event>>)((EntityEntry<Entity> x) => x.Entity.DomainEvents)).ToList();
				source.ToList().ForEach((Action<EntityEntry<Entity>>)delegate (EntityEntry<Entity> entity)
				{
					entity.Entity.ClearDomainEvents();
				});
				await Task.WhenAll(((IEnumerable<Event>)source2).Select((Func<Event, Task>)async delegate (Event domainEvent)
				{
					await _mediator.Publish(domainEvent);
				}));
			}
			return isSuccess;
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
