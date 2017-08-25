﻿using UnityEngine;

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DataManagement
{
    public static class DataBuilder
    {
        public static string Decrypt(string s)
        {
            if (DataManager.Instance.encrypt)
            {
                byte[] inputbuffer = System.Convert.FromBase64String(s);
                byte[] outputBuffer = DES.Create().CreateDecryptor(DataReferences.key, DataReferences.iv).TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                return Encoding.Unicode.GetString(outputBuffer);
            }
            else return s;
        }

        public static void BuildDataReferences()
        {
            var _dataManager = DataManager.Instance;
            var _path = Application.persistentDataPath + "/" + _dataManager.DataReferences.ID + "/" + _dataManager.DataReferences.ID + ".json";

            Debug.Log(_path);

            if (File.Exists(_path))
            {
                JsonUtility.FromJsonOverwrite(Decrypt(File.ReadAllText(_path)), _dataManager.DataReferences);
                Debug.Log("Building Data from: " + Application.persistentDataPath + "/" + _dataManager.DataReferences.ID);
            }
        }

        public static void BuildElementsOfType<T>(DataReferences.SavedElement saveData) where T : DataElement
        {
            for (int i = 0; i < saveData.ids.Count; i++)
            {
                if (saveData.types[i] == typeof(T).Name)
                    BuildElementOfType<T>(saveData, i);

                saveData.info[i].Build<T>();
            }
        }

        public static void BuildElementOfType<T>(DataReferences.SavedElement saveData, int index) where T : DataElement
        {
            var _id = saveData.ids[index].ToString();
            var _path = Application.persistentDataPath + "/" + DataManager.Instance.DataReferences.ID + "/" + _id + ".json";

            if (File.Exists(_path))
            {
                var _element = DataParser.CreateAsset<T>(_id) as T;
                JsonUtility.FromJsonOverwrite(Decrypt(File.ReadAllText(_path)), _element);

                saveData.info[index] = _element as T;
            }
        }
    }
}
