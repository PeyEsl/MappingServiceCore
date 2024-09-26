namespace MappingServiceCore.Mappings.ObjectToDictionaryMapping
{
    public class ObjectToDictionaryLinq
    {
        public Dictionary<string, object?> ObjectToDictionary<T>(T obj)
        {
            return obj!.GetType()
                      .GetProperties()
                      .ToDictionary(
                          prop => prop.Name,
                          prop =>
                          {
                              var value = prop.GetValue(obj);

                              if (prop.Name == "CreateDate" && value is DateTime)
                                  return ((DateTime)value).ToString();

                              return value;
                          });
        }

        public T DictionaryToObject<T>(Dictionary<string, object?> dict) where T : new()
        {
            var obj = new T();

            var objType = typeof(T);

            dict.ToList().ForEach(kvp =>
            {
                var prop = objType.GetProperty(kvp.Key);
                if (prop != null)
                {
                    if (kvp.Key == "CreateDate" && kvp.Value is string)
                        prop.SetValue(obj, DateTime.Parse((string)kvp.Value).ToString());
                    else
                        prop.SetValue(obj, kvp.Value);
                }
            });

            return obj;
        }
    }
}