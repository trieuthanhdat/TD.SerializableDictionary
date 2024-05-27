# SerializableDictionary

The `SerializableDictionary` is a custom implementation of a dictionary that can be serialized and deserialized by Unity. This is particularly useful for saving and loading data within Unity's editor and runtime environments. This implementation supports complex key and value types and ensures that all data is preserved across serialization cycles.

## Features

- **Serialization Support:** Allows dictionary data to be serialized and deserialized, making it suitable for use in Unity's inspector.
- **Key-Value Pair Management:** Provides methods for adding, removing, and accessing key-value pairs.
- **Collision Handling:** Handles key collisions by modifying the key to ensure uniqueness.
- **Clear and Count:** Supports clearing the dictionary and counting its elements.
- **Indexer:** Provides direct access to dictionary elements using an indexer.

## Implementation

The `SerializableDictionary` class is implemented as follows:

### Fields

- `m_KeyValuePairs`: A list of serializable key-value pairs.
- `m_Dictionary`: The internal dictionary that stores the actual key-value pairs.
- `ContentTable`: A property that exposes the internal dictionary.

### Methods

- **GetCount():** Returns the number of elements in the dictionary.
- **GetKeyAtIndex(int index):** Returns the key at the specified index in the list of key-value pairs.
- **GetValueAtIndex(int index):** Returns the value at the specified index in the list of key-value pairs.
- **OnBeforeSerialize():** Prepares the dictionary data for serialization.
- **OnAfterDeserialize():** Reconstructs the dictionary from the serialized data.
- **RemapListKeyPair(ref List<SerializableKeyValuePair> listKeyPair):** Filters and maps the list of key-value pairs to ensure consistency.
- **Add(TKey key, TValue value):** Adds a new key-value pair to the dictionary.
- **ModifyKeyToMakeUnique(TKey key):** Modifies the key to ensure it is unique within the dictionary.
- **ContainsKey(TKey key):** Checks if the dictionary contains the specified key.
- **ContainsValue(TValue value):** Checks if the dictionary contains the specified value.
- **Remove(TKey key):** Removes the key-value pair with the specified key from the dictionary.
- **Clear():** Clears the dictionary.
- **TryGetValue(TKey key, out TValue value):** Tries to get the value associated with the specified key.
- **Indexer:** Provides direct access to elements using a key.
- **CopyFromDictionary(Dictionary<TKey, TValue> source):** Creates a copy of a standard dictionary.
- **SerializableKeyValuePair Class:** A nested class representing a serializable key-value pair.

### Usage Example

```csharp
using UnityEngine;
using TD.SerializableDictionary;

public class ExampleUsage : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<string, int> serializableDictionary;

    void Start()
    {
        serializableDictionary = new SerializableDictionary<string, int>();
        serializableDictionary.Add("One", 1);
        serializableDictionary.Add("Two", 2);

        Debug.Log($"Count: {serializableDictionary.GetCount()}");
        Debug.Log($"Key at Index 0: {serializableDictionary.GetKeyAtIndex(0)}");
        Debug.Log($"Value at Index 0: {serializableDictionary.GetValueAtIndex(0)}");

        if (serializableDictionary.ContainsKey("Two"))
        {
            int value = serializableDictionary["Two"];
            Debug.Log($"Value for 'Two': {value}");
        }

        serializableDictionary.Clear();
        Debug.Log($"Count after clearing: {serializableDictionary.GetCount()}");
    }
}
