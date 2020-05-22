using System;
using Autofac;
using Resillience.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Resillience
{
	public abstract class CommonStartup
	{
		public IConfiguration Configuration { get; }
		public CommonStartup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		#region Core内置容器
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddControllers();
			services.Configure<ResillienceOption>(Configuration.GetSection("Resillience"));
			this.SupportServices(services);
		}
		public abstract void SupportServices(IServiceCollection services);
		#endregion

		#region 第三方容器Autofac
		public void ConfigureContainer(ContainerBuilder builder)
		{
			ResillienceContainer container = new ResillienceContainer(builder);
			this.SuppertContainer(container);
		}

		public abstract void SuppertContainer(ResillienceContainer container);
		#endregion



		//public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
		//{
		//	this.Run(app);
		//	app.UseRouting();
		//	app.UseEndpoints(endpoints=>
		//	{
		//		endpoints.MapControllers();
		//		this.MapEndpoints(endpoints);
		//	});
		//}
		
	}
}
