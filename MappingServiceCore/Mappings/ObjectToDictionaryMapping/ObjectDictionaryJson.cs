using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MappingServiceCore.Mappings.ObjectToDictionaryMapping
{
    public class ObjectDictionaryJson
    {
        public Dictionary<string, object?> ObjectToDictionary<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            var jObject = JObject.Parse(json);

            if (jObject.ContainsKey("CreateDate") && jObject["CreateDate"] != null)
            {
                var createDate = (DateTime)jObject["CreateDate"]!.ToObject(typeof(DateTime))!;

                jObject["CreateDate"] = createDate.ToString();
            }

            var dict = jObject.ToObject<Dictionary<string, object?>>();

            return dict!;
        }

        public T DictionaryToObject<T>(Dictionary<string, object?> dict)
        {
            var jObject = JObject.FromObject(dict);

            if (jObject.ContainsKey("CreateDate") && jObject["CreateDate"] != null)
            {
                var createDateString = jObject["CreateDate"]!.ToString();

                var createDate = DateTime.Parse(createDateString);

                jObject["CreateDate"] = createDate;
            }

            var json = jObject.ToString();

            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}