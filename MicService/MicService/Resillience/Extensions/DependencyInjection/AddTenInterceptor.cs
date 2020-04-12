using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Extensions.DependencyInjection
{
	public class AddTenInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			invocation.Proceed();
			if (invocation.Method.Name == "GetJ")
			{
				invocation.ReturnValue = 10 + (int)invocation.ReturnValue;
			}
		}
	}
}
