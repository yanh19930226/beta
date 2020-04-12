using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience
{
	public class ResillienceContainer
    {
		public ContainerBuilder Builder
		{
			get;
		}

		public ResillienceContainer(ContainerBuilder builder)
		{
			Builder = builder;
		}
	}
}
