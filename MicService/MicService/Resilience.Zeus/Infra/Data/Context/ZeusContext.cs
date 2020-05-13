using Microsoft.EntityFrameworkCore;
using Resilience.Zeus.Infra.Data.Map;
using Resillience;
using Resillience.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Resilience.Zeus.Infra.Data.Context
{
	public class ZeusContext : DbContext
	{
		public ZeusContext(DbContextOptions<ZeusContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (Type item in (Assembly.GetEntryAssembly()!.GetTypes()).Where(type => type.HasImplementedRawGeneric(typeof(ZeusEntityTypeConfiguration<>))))
			{
				dynamic val = Activator.CreateInstance(item);
				modelBuilder.ApplyConfiguration(val);
			}
			base.OnModelCreating(modelBuilder);
		}
	}
}
