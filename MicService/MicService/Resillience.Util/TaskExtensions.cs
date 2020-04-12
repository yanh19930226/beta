using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.Util
{
	public static class TaskExtensions
	{
		public static Task<T> InvokeAsync<T>(this Task<T> task) where T : class
		{
			return Task.Run<T>((Func<Task<T>>)(async () => await task));
		}

		public static ConfiguredTaskAwaitable<T> InvokeAsyncOnThrowContext<T>(this Task<T> task) where T : class
		{
			return Task.Run<T>((Func<Task<T>>)(async () => await task)).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
