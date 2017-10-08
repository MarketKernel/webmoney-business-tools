using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xml2WinForms.Templates
{
    public sealed class ControlTemplateJsonConverter : JsonConverter
    {
        private static volatile ConverterLogics _logics;

        public static ConverterLogics Logics
        {
            get => _logics ?? (_logics = new ConverterLogics());
            set => _logics = value ?? throw new ArgumentException(nameof(value));
        }

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
                return null;

            var jToken = JToken.Load(reader);

            return Logics.ReadJson(jToken, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
