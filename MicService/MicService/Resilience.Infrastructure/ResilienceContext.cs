using MediatR;
using Microsoft.EntityFrameworkCore;
using Resilience.Infrastructure.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resilience.Infrastructure
{
    public class ResilienceContext : DbContext, IUnitOfWork
    {
        private IMediator _mediator;
        public ResilienceContext(DbContextOptions<ResilienceContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }
        //public DbSet<Domain.AggregatesModel.ProjectVisibleRule> ProjectVisibleRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProjectPropertyConfiguration());

        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync();
            return true;
        }
    }
}
