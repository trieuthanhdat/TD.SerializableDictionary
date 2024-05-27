using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TD.SerializableDictionary
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair> m_KeyValuePairs = new List<SerializableKeyValuePair>();

        private Dictionary<TKey, TValue> m_Dictionary = new Dictionary<TKey, TValue>();
        public Dictionary<TKey, TValue> ContentTable { get { return m_Dictionary; } set { m_Dictionary = value; } }

        public int GetCount()
        {
            return m_Dictionary.Count;
        }
        public TKey GetKeyAtIndex(int index)
        {
            if (m_KeyValuePairs == null) return default;
            var keyPair = m_KeyValuePairs[index];
            return keyPair != null ? keyPair.Key : default;
        }
        public TValue GetValueAtIndex(int index)
        {
            if (m_KeyValuePairs == null) return default;
            var keyPair = m_KeyValuePairs[index];
            return keyPair != null ? keyPair.Value : default;
        }
        public void OnBeforeSerialize()
        {
            m_KeyValuePairs.Clear();
            foreach (var pair in m_Dictionary)
            {
                m_KeyValuePairs.Add(new SerializableKeyValuePair(pair.Key, pair.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            m_Dictionary = new Dictionary<TKey, TValue>();
            RemapListKeyPair(ref m_KeyValuePairs);
            // Create a copy of the keyValuePairs list to avoid modification during iteration
            List<SerializableKeyValuePair> tmpKeypair = new List<SerializableKeyValuePair>(m_KeyValuePairs);
            foreach (var pair in tmpKeypair)
            {
                var nextKey = pair.Key;
                Add(nextKey, pair.Value);
            }
        }
        private void RemapListKeyPair(ref List<SerializableKeyValuePair> listKeyPair)
        {
            List<SerializableKeyValuePair> listToMap = new List<SerializableKeyValuePair>();

            foreach (var pair in listKeyPair)
            {
                if(ContentTable.ContainsKey(pair.Key))
                {
                    listToMap.Add(pair);
                }
            }
            if (listToMap.Count == 0) return;
            listKeyPair.Clear();
            listKeyPair.AddRange(listToMap);
            listKeyPair.Distinct();
        }
        
        public void Add(TKey key, TValue value)
        {
            // Check if the key already exists before adding
            if (!m_Dictionary.ContainsKey(key))
            {
                m_Dictionary.Add(key, value);
                m_KeyValuePairs.Add(new SerializableKeyValuePair(key, value));
                Debug.Log($"Key '{key}' is new. Adding with a new key.");
            }
            else
            {
                // Add the entry with the modified key after a delay
                Task.Delay(TimeSpan.FromSeconds(0.3)).ContinueWith(_ =>
                {
                    // Handle the case where the key already exists
                    Debug.Log($"Key '{key}' already exists in the dictionary. Adding with a modified key.");
                    // Modify the key to make it unique
                    TKey modifiedKey = ModifyKeyToMakeUnique(key);
                    m_Dictionary.Add(modifiedKey, value);
                    m_KeyValuePairs.Add(new SerializableKeyValuePair(modifiedKey, value));
                });
            }
        }

        private Dictionary<TKey, int> _keyCollisionCount = new Dictionary<TKey, int>();
        private TKey ModifyKeyToMakeUnique(TKey key)
        {
            int collisionCount;
            TKey nextKey;

            // Remove entries from keyCollisionCount that are not in _dictionary
            List<TKey> keysToRemove = _keyCollisionCount.Keys.Where(k => !m_Dictionary.ContainsKey(k)).ToList();
            foreach (var keyToRemove in keysToRemove)
            {
                _keyCollisionCount.Remove(keyToRemove);
            }

            do
            {
                collisionCount = _keyCollisionCount.TryGetValue(key, out int count) ? count + 1 : 1;
                _keyCollisionCount[key] = collisionCount;

                if (typeof(TKey) == typeof(string))
                {
                    string stringKey = key.ToString();
                    nextKey          = (TKey)(object)(stringKey + "_" + collisionCount);
                }
                else if (typeof(TKey) == typeof(int))
                {
                    int intKey = (int)(object)key;
                    nextKey    = (TKey)(object)(intKey + collisionCount);
                }
                else if (typeof(TKey) == typeof(float))
                {
                    float floatKey = (float)(object)key;
                    nextKey        = (TKey)(object)(floatKey + 0.1f * collisionCount);
                }
                else if (typeof(TKey) == typeof(long) ||
                         typeof(TKey) == typeof(ulong))
                {
                    long longKey = (long)(object)key;
                    nextKey      = (TKey)(object)(longKey + collisionCount);
                }
                else if (typeof(TKey) == typeof(char))
                {
                    char charKey = (char)(object)key;
                    nextKey      = (TKey)(object)(charKey == char.MaxValue ? char.MinValue : (char)(charKey + collisionCount));
                }
                else if (typeof(TKey) == typeof(double))
                {
                    double doubleKey = (double)(object)key;
                    nextKey          = (TKey)(object)(doubleKey + 0.1 * collisionCount);
                }
                else if (typeof(TKey) == typeof(bool))
                {
                    // Toggle the boolean value
                    nextKey = (TKey)(object)(!(bool)(object)key);
                }
                else if (typeof(TKey).IsEnum)
                {
                    // Handle enum types by converting to the underlying type, modifying, and converting back
                    var underlyingType = Enum.GetUnderlyingType(typeof(TKey));
                    if (underlyingType == typeof(int))
                    {
                        int intKey = (int)(object)key;
                        intKey    += collisionCount;
                        nextKey    = (TKey)Enum.ToObject(typeof(TKey), intKey);
                    }
                    else if (underlyingType == typeof(byte))
                    {
                        byte byteKey = (byte)(object)key;
                        byteKey     += (byte)collisionCount;
                        nextKey      = (TKey)Enum.ToObject(typeof(TKey), byteKey);
                    }
                    else if (underlyingType == typeof(long))
                    {
                        long longKey = (long)(object)key;
                        longKey     += collisionCount;
                        nextKey      = (TKey)Enum.ToObject(typeof(TKey), longKey);
                    }
                    else
                    {
                        // Add other underlying types as necessary
                        Debug.LogError($"Unsupported underlying enum type: {underlyingType}");
                        return key;
                    }
                }
                else
                {
                    // Handle other types as needed
                    Debug.LogError($"Unsupported key type: {typeof(TKey)}");
                    return key;
                }
            } while (m_Dictionary.ContainsKey(nextKey));

            return nextKey;
        }



        public bool ContainsKey(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }
        public bool ContainsValue(TValue value)
        {
            return m_Dictionary.ContainsValue(value);
        }


        public bool Remove(TKey key)
        {
            if (m_Dictionary.TryGetValue(key, out _))
            {
                m_Dictionary.Remove(key);
                m_KeyValuePairs.RemoveAll(pair => EqualityComparer<TKey>.Default.Equals(pair.Key, key));
                return true;
            }
            return false;
        }

        public void Clear()
        {
            m_Dictionary.Clear();
            m_KeyValuePairs.Clear();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_Dictionary.TryGetValue(key, out value);
        }


        // Add indexer for direct access by key
        public TValue this[TKey key]
        {
            get => m_Dictionary[key];
            set => m_Dictionary[key] = value;
        }

        // Copy constructor method
        public static SerializableDictionary<TKey, TValue> CopyFromDictionary(Dictionary<TKey, TValue> source)
        {
            var newDictionary = new SerializableDictionary<TKey, TValue>();
            foreach (var pair in source)
            {
                newDictionary.Add(pair.Key, pair.Value);
            }
            return newDictionary;
        }

        [Serializable]
        public class SerializableKeyValuePair
        {
            [SerializeField]
            private TKey key;
            [SerializeField]
            private TValue value;

            public SerializableKeyValuePair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }

            public TKey Key => key;
            public TValue Value => value;

            public override string ToString()
            {
                return base.ToString() + " => key: " + key + " value: " + value;
            }
        }

        // Ensure the dictionary is initialized before use
        public SerializableDictionary()
        {
            m_Dictionary = new Dictionary<TKey, TValue>();
            m_KeyValuePairs = new List<SerializableKeyValuePair>();
        }
    }
}
