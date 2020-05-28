using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.Util
{
	public static class JsonExtensions
	{
		public static string ToJson(this object self, bool ignoreNull = false)
		{
			return JsonConvert.SerializeObject(self, Formatting.None, new JsonSerializerSettings
			{
				DateFormatString = "yyyy-MM-dd HH:mm:ss",
				NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
			});
		}

		public static T DeJson<T>(this string self)
		{
			return self.IsNullOrEmpty() ? default : JsonConvert.DeserializeObject<T>(self);
		}

        /// <summary>
        /// 根据key将json文件内容转换为指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> FromJsonFile<T>(this string filePath, string key = "") where T : new()
        {
            if (!File.Exists(filePath)) return new T();

            using StreamReader reader = new StreamReader(filePath);
            var json = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(key)) return JsonConvert.DeserializeObject<T>(json);

            return !(JsonConvert.DeserializeObject<object>(json) is JObject obj) ? new T() : JsonConvert.DeserializeObject<T>(obj[key].ToString());
        }
    }
}
