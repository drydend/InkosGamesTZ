using System.Collections.Generic;

namespace Utils
{
    public static class DictionaryExtentions
    {
        public static void Add<Key, Value>(this Dictionary<Key, Value> origin, Dictionary<Key, Value> dictionary)
        {
            foreach (var keyValuePair in dictionary)
            {
                if (origin.ContainsKey(keyValuePair.Key))
                {
                    continue;
                }

                origin.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
