using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zpi_aspnet_test.Extensions
{
   public static class DictionaryExtensions
   {
      public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
      {
         key = tuple.Key;
         value = tuple.Value;
      }

      public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defValue = default)
         => dict.TryGetValue(key, out var value) ? value : defValue;
   }
}