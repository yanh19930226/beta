using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience
{
	public class ResillienceBuilder
    {
		public IServiceCollection Services
		{
			get;
			set;
		}

		public ResillienceBuilder(IServiceCollection services)
		{
			Services = services;
		}

		public IApplicationBuilder App
		{
			get;
			set;
		}

		public ResillienceBuilder(IApplicationBuilder app)
		{
			App = app;
		}
	}
}
