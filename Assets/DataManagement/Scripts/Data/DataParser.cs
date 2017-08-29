﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace DataManagement
{
    public static class DataParser
    {
        public static string Encrypt(string p_input)
        {
            if (DataManager.Instance.encrypt)
            {
                var t_inputbuffer = Encoding.Unicode.GetBytes(p_input);
                var t_outputBuffer = DES.Create().CreateEncryptor(DataReferences.key, DataReferences.iv).TransformFinalBlock(t_inputbuffer, 0, t_inputbuffer.Length);
                return System.Convert.ToBase64String(t_outputBuffer);
            }
            else return p_input;
        }

        public static ScriptableObject CreateAsset<T>(string p_name) where T : ScriptableObject
        {
            var t_asset = ScriptableObject.CreateInstance<T>() as T;

            #if UNITY_EDITOR
            AssetDatabase.CreateAsset(t_asset, "Assets/Temp_Assets/" + p_name + ".asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = t_asset;
            #endif

            return t_asset;
        }

        public static void SaveJSON(string p_name, string p_info)
        {
            var t_path = Application.persistentDataPath + "/" + DataManager.Instance.DataReferences.ID + "/" + p_name + ".json";
            if (!File.Exists(t_path)) File.Delete(t_path);

            using (FileStream fs = new FileStream(t_path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                    writer.Write(Encrypt(p_info));
            }
        }
    }
}

