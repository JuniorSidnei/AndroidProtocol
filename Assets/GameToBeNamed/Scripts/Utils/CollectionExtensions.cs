using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameToBeNamed.Utils {
    public static class CollectionExtensions {


        public static TValue IsSet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) {
            if (!dictionary.ContainsKey(key)) {
                return default(TValue);
            }
            return dictionary[key];
        }

        public static IEnumerable<TValue> AllNotDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys) {
            return from key in keys where !dictionary.IsSet(key).Equals(default(TValue)) select dictionary[key];
        }
    }
}