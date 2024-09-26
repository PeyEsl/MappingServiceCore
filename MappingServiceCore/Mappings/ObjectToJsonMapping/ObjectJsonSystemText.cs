using System.Text.Json;

namespace MappingServiceCore.Mappings.ObjectToJsonMapping
{
    public class ObjectJsonSystemText
    {
        public string ObjectToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        }

        public T JsonToObject<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json)!;
        }
    }
}