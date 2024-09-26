namespace MappingServiceCore.Mappings.ObjectToDictionaryMapping
{
    public class ObjectDictionaryReflection
    {
        public Dictionary<string, object?> ObjectToDictionary(object obj)
        {
            var dict = new Dictionary<string, object?>();

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj, null);

                if (property.Name == "CreateDate" && value is DateTime)
                {
                    dict.Add(property.Name, ((DateTime)value).ToString());
                }
                else
                {
                    dict.Add(property.Name, value);
                }
            }

            return dict;
        }

        public T DictionaryToObject<T>(Dictionary<string, object?> dict) where T : new()
        {
            var obj = new T();
            foreach (var property in typeof(T).GetProperties())
            {
                if (dict.ContainsKey(property.Name))
                {
                    var value = dict[property.Name];

                    if (property.Name == "CreateDate" && value is string)
                    {
                        property.SetValue(obj, DateTime.Parse((string)value).ToString());
                    }
                    else
                    {
                        property.SetValue(obj, value);
                    }
                }
            }
            return obj;
        }
    }
}