using Autofac;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Resilience.Zeus.Domain.Core.Bus;
using Resilience.Zeus.Domain.Interfaces;
using Resilience.Zeus.Infra.Data.Context;
using Resilience.Zeus.Infra.Data.Repository;
using Resilience.Zeus.Infra.Data.Uow;
using Resillience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Resilience.Zeus
{
	public static class DependencyInjection
	{
		public static ResillienceContainer EnableZeus(this ResillienceContainer container, IConfiguration configuration)
		{
			ZeusOptions zeusOptions = ConfigurationBinder.Get<ZeusOptions>((IConfiguration)(object)configuration.GetSection("Resillience:Zeus"));
			DbContextOptionsBuilder<ZeusContext> optionsBuilder = new DbContextOptionsBuilder<ZeusContext>();
			optionsBuilder.UseMySql(zeusOptions.Connection, b => b.MigrationsAssembly("ServiceB"));
			container.Builder.RegisterType<ZeusContext>().AsSelf().InstancePerLifetimeScope();
			container.Builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
			container.Builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
			container.Builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Queries")).AsImplementedInterfaces();
			container.Builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsClosedTypesOf(typeof(IRequestHandler<,>));
			container.Builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsClosedTypesOf(typeof(INotificationHandler<>));
			container.Builder.RegisterType<InMemoryBus>().As<IMediatorHandler>().InstancePerLifetimeScope();
			container.Builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

			container.Builder.Register((Func<IComponentContext, ServiceFactory>)delegate (IComponentContext context)
			{
				IComponentContext c2 = context.Resolve<IComponentContext>();
				return (Type t) => c2.Resolve(t);
			});

			#region Mapping

			List<Profile> autoMapperProfiles = (Assembly.GetEntryAssembly()!.GetTypes()).Where(p => p.BaseType == typeof(Profile))
			.Select(p => (Profile)Activator.CreateInstance(p)).ToList();

			container.Builder.Register(ctx=> new MapperConfiguration(cfg=>
			{
				foreach (Profile item in autoMapperProfiles)
				{
					cfg.AddProfile(item);
				}
			}));
			container.Builder.Register(ctx=> ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
			
			#endregion

			return container;
		}
	}
}
