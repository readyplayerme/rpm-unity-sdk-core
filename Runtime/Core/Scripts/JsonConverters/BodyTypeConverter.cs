using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace ReadyPlayerMe.Core
{
    [Preserve]
    public class BodyTypeConverter : JsonConverter<BodyType>
    {
        public override void WriteJson(JsonWriter writer, BodyType value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString().ToLower());
        }

        public override BodyType ReadJson(JsonReader reader, Type objectType, BodyType existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.String)
            {
                return EnumExtensions.GetValueFromDescription<BodyType>(token.ToString());
            }

            throw new JsonSerializationException("Expected string value, instead found: " + token.Type);
        }
    }
}
