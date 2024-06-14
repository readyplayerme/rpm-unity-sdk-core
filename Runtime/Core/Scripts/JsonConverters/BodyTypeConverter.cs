using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace ReadyPlayerMe.Core
{
    [Preserve]
    public class BodyTypeConverter : JsonConverter<BodyType>
    {
        private const string FULL_BODY = "fullbody";
        private const string FULL_BODY_XR = "fullbody-xr";
        private const string HALF_BODY = "halfbody";

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

                return token.ToString() switch
                {
                    FULL_BODY => BodyType.FullBody,
                    HALF_BODY => BodyType.HalfBody,
                    FULL_BODY_XR => BodyType.FullBodyXR,
                    _ => BodyType.None
                };
            }

            // This is a fallback to the previous SDK versions, where the bodyType was stored as an Integer.
            if (token.Type == JTokenType.Integer)
            {
                return (BodyType) token.Value<int>();
            }

            throw new JsonSerializationException("Expected string or integer value, instead found: " + token.Type);
        }
    }
}
