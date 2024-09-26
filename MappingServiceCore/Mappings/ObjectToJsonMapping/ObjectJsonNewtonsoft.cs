using Newtonsoft.Json;

namespace MappingServiceCore.Mappings.ObjectToJsonMapping
{
    public class ObjectJsonNewtonsoft
    {
        public string ObjectToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T JsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}