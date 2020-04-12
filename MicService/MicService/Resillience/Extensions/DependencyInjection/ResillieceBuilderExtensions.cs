using Castle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Extensions.DependencyInjection
{
	public static class ResillienceBuilderExtensions
	{
		//public static IServiceCollection AddDemonAuthentication(this IServiceCollection services, string authorityAddress)
		//{
		//	return services.AddDemonAuthentication((Action<DemonAuthenticationOptions>)delegate (DemonAuthenticationOptions o)
		//	{
		//		o.AuthorizationUrl = authorityAddress;
		//	});
		//}

		//public static IServiceCollection AddDemonAuthentication(this IServiceCollection services, Action<DemonAuthenticationOptions> configure = null)
		//{
		//	DemonAuthenticationOptions ms_options = new DemonAuthenticationOptions();
		//	DemonOptions service = ServiceProviderServiceExtensions.GetService<DemonOptions>((IServiceProvider)ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services));
		//	if (service == null)
		//	{
		//		throw new MemberAccessException("请先执行AddDemon方法再执行此方法");
		//	}
		//	if (string.IsNullOrEmpty(ms_options.ApiName))
		//	{
		//		ms_options.ApiName = service.DistributedName;
		//	}
		//	configure?.Invoke(ms_options);
		//	AuthenticationServiceCollectionExtensions.AddAuthentication(services, (Action<AuthenticationOptions>)delegate (AuthenticationOptions options)
		//	{
		//		options.set_DefaultScheme("Bearer");
		//		options.set_DefaultAuthenticateScheme("Bearer");
		//		options.set_DefaultChallengeScheme("Bearer");
		//	}).AddIdentityServerAuthentication((Action<IdentityServerAuthenticationOptions>)delegate (IdentityServerAuthenticationOptions options)
		//	{
		//		options.Authority = ms_options.AuthorizationUrl;
		//		options.ApiName = ms_options.ApiName;
		//		options.RequireHttpsMetadata = false;
		//	});
		//	return services;
		//}

		//public static DemonBuilder AddSwagger(this DemonBuilder builder, Action<DemonSwaggerOptions> setupAction)
		//{
		//	if (setupAction == null)
		//	{
		//		setupAction = delegate
		//		{
		//		};
		//	}
		//	IServiceCollection services = builder.Services;
		//	DemonSwaggerOptions obj = new DemonSwaggerOptions();
		//	setupAction?.Invoke(obj);
		//	OptionsServiceCollectionExtensions.Configure<DemonSwaggerOptions>(services, setupAction).AddSwaggerCore();
		//	return builder;
		//}

		//public static DemonBuilder AddDemonSwagger(this DemonBuilder builder, IConfiguration configuration = null)
		//{
		//	configuration = (configuration ?? ServiceProviderServiceExtensions.GetService<IConfiguration>((IServiceProvider)ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(builder.Services)));
		//	IConfigurationSection section = configuration.GetSection("Demon:Swagger");
		//	OptionsConfigurationServiceCollectionExtensions.Configure<DemonSwaggerOptions>(builder.Services, (IConfiguration)(object)section).AddSwaggerCore();
		//	return builder;
		//}

		//public static DemonBuilder AddSwagger(this DemonBuilder builder, IConfiguration configuration = null)
		//{
		//	configuration = (configuration ?? ServiceProviderServiceExtensions.GetService<IConfiguration>((IServiceProvider)ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(builder.Services)));
		//	IConfigurationSection section = configuration.GetSection("Demon:Swagger");
		//	OptionsConfigurationServiceCollectionExtensions.Configure<DemonSwaggerOptions>(builder.Services, (IConfiguration)(object)section).AddSwaggerCore();
		//	return builder;
		//}

		//private static void AddSwaggerCore(this IServiceCollection services)
		//{
		//	_ = DistributedCoreConfig.DistributedName;
		//	ServiceProvider val = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
		//	DemonSwaggerOptions j_options = ServiceProviderServiceExtensions.GetService<IOptions<DemonSwaggerOptions>>((IServiceProvider)val).get_Value();
		//	if (j_options.ApiVersionItems == null || j_options.ApiVersionItems.Count == 0)
		//	{
		//		j_options.SingleApiVersion(new ApiVersionItem(1, "HTTP API", $"v{1}"));
		//	}
		//	services.AddSwaggerGen((Action<SwaggerGenOptions>)delegate (SwaggerGenOptions s_options)
		//	{
		//		s_options.DescribeAllEnumsAsStrings();
		//		s_options.DescribeAllParametersInCamelCase();
		//		foreach (ApiVersionItem apiVersionItem in j_options.ApiVersionItems)
		//		{
		//			OpenApiInfo info = new OpenApiInfo
		//			{
		//				Title = apiVersionItem.Title,
		//				Version = $"v{apiVersionItem.Version}",
		//				Description = apiVersionItem.Description
		//			};
		//			s_options.SwaggerDoc($"v{apiVersionItem.Version}", info);
		//		}
		//		string.IsNullOrEmpty(j_options.AuthorizationUrl);
		//		s_options.DocInclusionPredicate((Func<string, ApiDescription, bool>)delegate (string docName, ApiDescription apiDesc)
		//		{
		//			IEnumerable<ApiVersion> source = apiDesc.CustomAttributes().OfType<DemonApiVersionAttribute>().SelectMany((Func<DemonApiVersionAttribute, IEnumerable<ApiVersion>>)((DemonApiVersionAttribute attr) => attr.Versions));
		//			apiDesc.CustomAttributes().OfType<RouteAttribute>().Select((Func<RouteAttribute, bool>)((RouteAttribute s) => s.get_Template() == "api/v{version:apiVersion}/[controller]"));
		//			return source.Any((Func<ApiVersion, bool>)((ApiVersion v) => "v" + v.MajorVersion.ToString() == docName));
		//		});
		//		s_options.OperationFilter<AuthorizeCheckOperationFilter>(Array.Empty<object>());
		//		s_options.OperationFilter<InternalServerErrorOperationFilter>(Array.Empty<object>());
		//		s_options.OperationFilter<RemoveVersionParametersOperationFilter>(Array.Empty<object>());
		//		s_options.OperationFilter<RemoveLanguageParametersOperationFilter>(Array.Empty<object>());
		//		s_options.OperationFilter<RemoveObsoleteQueryParametersOperationFilter>(Array.Empty<object>());
		//		s_options.DocumentFilter<SetVersionInPaths>(Array.Empty<object>());
		//		s_options.DocumentFilter<SetLanguageInPaths>(Array.Empty<object>());
		//		string[] files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
		//		if (files.Length != 0)
		//		{
		//			for (int i = 0; i < files.Length; i++)
		//			{
		//				s_options.IncludeXmlComments(files[i]);
		//			}
		//		}
		//		s_options.EnableAnnotations();
		//	});
		//}
	}
}
