using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.SerializableDictionary
{
    public class TestSerializeDictionary : MonoBehaviour
    {
        public enum TestEnumKey
        {
            NONE,
            TEST_1,
            TEST_2,
            TEST_3
        }
        public SerializableDictionary<int,    int> integerKeyTable  = new SerializableDictionary<int,    int>();
        public SerializableDictionary<string, int> stringKeyTable   = new SerializableDictionary<string, int>();
        public SerializableDictionary<long,   int> longKeyTable     = new SerializableDictionary<long,   int>();
        public SerializableDictionary<float,  int> floatKeyTable    = new SerializableDictionary<float,  int>();
        public SerializableDictionary<bool,   int> boolKeyTable     = new SerializableDictionary<bool,   int>();
        public SerializableDictionary<char,   int> charKeyTable     = new SerializableDictionary<char,   int>();
        public SerializableDictionary<double, int> doubleKeyTable   = new SerializableDictionary<double, int>();
        public SerializableDictionary<TestEnumKey,int> enumKeyTable = new SerializableDictionary<TestEnumKey, int>();

        private string DataFolder = "DataResource";
        private string DataFile = "TestSerializeDictionary";
#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                string jsonInt = JsonUtility.ToJson(integerKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonInt);
                SaveAndLoad<SerializableDictionary<int, int>>.SaveWithJsonConvert(integerKeyTable.ContentTable, DataFolder, DataFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                string jsonString = JsonUtility.ToJson(stringKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonString);
                SaveAndLoad<SerializableDictionary<string, int>>.SaveWithJsonConvert(stringKeyTable.ContentTable, DataFolder, DataFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                string jsonLong = JsonUtility.ToJson(longKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonLong);
                SaveAndLoad<SerializableDictionary<long, int>>.SaveWithJsonConvert(longKeyTable.ContentTable, DataFolder, DataFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                string jsonFloat = JsonUtility.ToJson(floatKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonFloat);
                SaveAndLoad<SerializableDictionary<float, int>>.SaveWithJsonConvert(floatKeyTable.ContentTable, DataFolder, DataFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                string jsonBool = JsonUtility.ToJson(boolKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonBool);
                SaveAndLoad<SerializableDictionary<bool, int>>.SaveWithJsonConvert(boolKeyTable.ContentTable, DataFolder, DataFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                string jsonChar = JsonUtility.ToJson(charKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonChar);
                SaveAndLoad<SerializableDictionary<char, int>>.SaveWithJsonConvert(charKeyTable.ContentTable, DataFolder, DataFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                string jsonDouble = JsonUtility.ToJson(doubleKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonDouble);
                SaveAndLoad<SerializableDictionary<double, int>>.SaveWithJsonConvert(doubleKeyTable.ContentTable, DataFolder, DataFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                string jsonEnum = JsonUtility.ToJson(enumKeyTable);
                Debug.Log("TEST SERIALIZE DICTIONARY: " + jsonEnum);
                SaveAndLoad<SerializableDictionary<TestEnumKey, int>>.SaveWithJsonConvert(enumKeyTable.ContentTable, DataFolder, DataFile);
            }
        }
#endif
    }
}
