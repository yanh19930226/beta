using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Resillience.Extensions.DependencyInjection
{
	public class ProxyGenerationHook : IProxyGenerationHook
	{
		public void MethodsInspected()
		{
			throw new NotImplementedException();
		}

		public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
		{
			throw new NotImplementedException();
		}

		public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
		{
			throw new NotImplementedException();
		}
	}
}
