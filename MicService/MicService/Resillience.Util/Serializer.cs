using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Resillience.Util
{
	public static class Serializer
	{
		public static byte[] SerializeBytes(object value)
		{
			if (value == null)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize(memoryStream, value);
			return memoryStream.ToArray();
		}

		public static object DeserializeBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			MemoryStream serializationStream = new MemoryStream(bytes);
			return new BinaryFormatter().Deserialize(serializationStream);
		}
	}
}
