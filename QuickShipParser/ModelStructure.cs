using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickShipParser
{
    public class ModelStructure
    {
        public string ModelName { get; set; }
        public string BaseModel { get; set; }
        public List<Element> Elements { get; set; }

        // Static method to handle the deserialization
        public static ModelStructure FromJson(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new ArgumentException("The provided JSON string is null or empty.", nameof(jsonString));
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ModelStructure>(jsonString, options) ?? throw new InvalidOperationException("Deserialization of the JSON string failed.");
        }

        public class Element : IPattern
        {
            public string CodeName { get; set; }
            public bool Optional { get; set; }
            public List<CodeDescription> Codes { get; set; }

            public IMatch Match(string text)
            {
                if (Optional)
                {
                    var pattern = new OptionalPattern(new Choice(Codes.ToArray()));
                    return pattern.Match(text);
                }
                else
                {
                    var pattern = new Choice(Codes.ToArray());
                    return pattern.Match(text);
                }
            }
        }

        public class CodeDescription : IPattern
        {
            public string Code { get; set; }
            public string Description { get; set; }

            public IMatch Match(string text)
            {
                var pattern = new JsonText(Code);
                return pattern.Match(text);
            }
        }
    }
}