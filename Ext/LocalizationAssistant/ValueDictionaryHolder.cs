using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace LocalizationAssistant
{
    internal sealed class ValueDictionaryHolder
    {
        public SortedDictionary<string, string> Dictionary { get; }
        public bool Changed { get; set; }

        public ValueDictionaryHolder()
        {
            Dictionary = new SortedDictionary<string, string>();
        }

        public ValueDictionaryHolder(SortedDictionary<string, string> dictionary)
        {
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public static ValueDictionaryHolder LoadFormFile(string filePath)
        {
            if (null == filePath)
                throw new ArgumentNullException(nameof(filePath));

            SortedDictionary<string, string> dictionary;

            var serializer = new JsonSerializer();

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                try
                {
                    dictionary =
                        serializer
                            .Deserialize<SortedDictionary<string, string>>(jsonTextReader);
                }
                catch (JsonException)
                {
                    dictionary = new SortedDictionary<string, string>();
                }
            }

            if (null == dictionary)
                dictionary = new SortedDictionary<string, string>();

            return new ValueDictionaryHolder(dictionary);
        }

        public void SaveToFile(string filePath)
        {
            if (null == filePath)
                throw new ArgumentNullException(nameof(filePath));

            var serializer = new JsonSerializer();

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter) {Formatting = Formatting.Indented})
            {
                serializer.Serialize(jsonTextWriter, Dictionary);

                jsonTextWriter.Flush();
                streamWriter.Flush();
                fileStream.Flush(true);
            }
        }
    }
}
