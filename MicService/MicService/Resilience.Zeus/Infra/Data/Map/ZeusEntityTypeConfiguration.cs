using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resilience.Zeus.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Resilience.Zeus.Infra.Data.Map
{
	public abstract class ZeusEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
	{
		public void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasKey(p => p.Id);
			Config(builder);
		}

		public abstract void Config(EntityTypeBuilder<TEntity> builder);
	}
}
