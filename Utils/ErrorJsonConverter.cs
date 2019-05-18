using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DDDCommon.Utils
{
    public class ErrorJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Error);

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is Error error))
                throw new JsonSerializationException("expected Error object");

            var o = new JObject
            {
                { "code", error.Code },
                { "text", error.Text },
                { "details", error.Details }
            };
            o.WriteTo(writer);
        }
    }

}
