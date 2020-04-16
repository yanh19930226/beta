using Autofac;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Resillience.Options;

namespace Resillience
{
	public abstract class CommonStartup
	{
		public CommonStartup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<ResillienceOption>(this.Configuration.GetSection("Resillience"));
			services.Configure<ResillienceOption>(this.Configuration.GetSection("Zeus"));
			this.SupportServices(services);
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			ResillienceContainer container = new ResillienceContainer(builder);
			this.SuppertContainer(container);
		}

		public abstract void SupportServices(IServiceCollection services);

		public virtual void SuppertContainer(ResillienceContainer container)
		{
			//container.EnableDemonApiController(null).EnableDemonService(null, "").EnableDemonDAO(null, "");
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseMiddleware(Array.Empty<object>());
			app.UseRouting();
			this.Run(app);
			app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
			{
				endpoints.MapControllers();
				this.MapEndpoints(endpoints);
			});
		}

		public virtual void MapEndpoints(IEndpointRouteBuilder endpoints)
		{
		}

		protected abstract void Run(IApplicationBuilder app);
	}
}
