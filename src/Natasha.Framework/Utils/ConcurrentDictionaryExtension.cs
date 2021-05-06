using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ConcurrentDictionaryExtension
{
    public static S Remove<T, S>(this ConcurrentDictionary<T, S> dict, T key)
    {

        while (!dict.TryRemove(key, out var result))
        {
            if (!dict.ContainsKey(key))
            {
                return result;
            }
        }
        return default;
    }
}

