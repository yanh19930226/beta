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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Resilience.Zeus
{
	public static class DependencyInjection
	{
		public static DemonContainer EnableZeus(this DemonContainer container, IConfiguration configuration)
		{
			ZeusOptions zeusOptions = ConfigurationBinder.Get<ZeusOptions>((IConfiguration)(object)configuration.GetSection("Zeus"));
			DbContextOptionsBuilder<ZeusContext> optionsBuilder = new DbContextOptionsBuilder<ZeusContext>();
			container.Builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
			optionsBuilder.UseMySql(zeusOptions.Connection);
			container.Builder.Register((Func<IComponentContext, ZeusContext>)((IComponentContext c) => new ZeusContext(optionsBuilder.Options))).As<ZeusContext>().InstancePerLifetimeScope();
			container.Builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
			container.Builder.RegisterType<InMemoryBus>().As<IMediatorHandler>().InstancePerLifetimeScope();
			container.Builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).Where((Func<Type, bool>)((Type t) => t.Name.EndsWith("Queries"))).AsImplementedInterfaces()
				.InstancePerLifetimeScope();
			container.Builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
			container.Builder.Register((Func<IComponentContext, ServiceFactory>)delegate (IComponentContext context)
			{
				IComponentContext c2 = context.Resolve<IComponentContext>();
				return (Type t) => c2.Resolve(t);
			});
			container.Builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsClosedTypesOf(typeof(IRequestHandler<,>));
			container.Builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsClosedTypesOf(typeof(INotificationHandler<>));
			List<Profile> autoMapperProfiles = ((IEnumerable<Type>)Assembly.GetEntryAssembly()!.GetTypes()).Where((Func<Type, bool>)((Type p) => p.BaseType == typeof(Profile))).Select((Func<Type, Profile>)((Type p) => (Profile)Activator.CreateInstance(p))).ToList();
			container.Builder.Register((Func<IComponentContext, MapperConfiguration>)((IComponentContext ctx) => new MapperConfiguration((Action<IMapperConfigurationExpression>)delegate (IMapperConfigurationExpression cfg)
			{
				foreach (Profile item in autoMapperProfiles)
				{
					cfg.AddProfile(item);
				}
			})));
			container.Builder.Register((Func<IComponentContext, IMapper>)((IComponentContext ctx) => ctx.Resolve<MapperConfiguration>().CreateMapper())).As<IMapper>().InstancePerLifetimeScope();
			return container;
		}
	}
}
