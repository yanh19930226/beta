using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Test
{
	public static class ResillienceBuilderExtensions
	{
		public static ResillienceBuilder AddTest(this ResillienceBuilder builder, IConfiguration configuration = null)
		{
			IServiceCollection services = builder.Services;
			services.AddSingleton<ITestService, TestService>();
			return builder;
		}
	}
}
