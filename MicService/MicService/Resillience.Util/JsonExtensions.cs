using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util
{
	public static class JsonExtensions
	{
		public static string ToJson(this object self)
		{
			return JsonConvert.SerializeObject(self);
		}

		public static T DeJson<T>(this string self)
		{
			return JsonConvert.DeserializeObject<T>(self);
		}
	}
}
