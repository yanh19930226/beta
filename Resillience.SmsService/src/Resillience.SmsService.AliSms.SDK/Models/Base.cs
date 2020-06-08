using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.AliSms.SDK.Models
{
    public abstract class BaseRequest<T>
    {
        public string Format { get; } = "JSON";
        public string RegionId { get; } = "cn-hangzhou";
        public abstract string Action { get; }
        public string SignatureMethod { get; } = "HMAC-SHA1";
        public string SignatureNonce { get; } = new Random().Next(1000, 9999).ToString();
        public string SignatureVersion { get; } = "1.0";
        public string Timestamp { get; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        public string Version { get; } = "2017-05-25";
    }

    public class BaseResponse<T>
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string RequestId { get; set; }

        [JsonConverter(typeof(DataConvert))]
        public T result { get; set; }
    }


    public class DataConvert : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var result = serializer.Deserialize(reader);

                if (result.ToString() == "[]")
                    return null;

                return JsonConvert.DeserializeObject(result.ToString(), objectType);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Console.WriteLine(111);
        }
    }
}
