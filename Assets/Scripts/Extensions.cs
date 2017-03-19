﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Extensions
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static ScriptableObject CreateAsset<T>(string name, string path) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        #if UNITY_EDITOR
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        #endif

        return asset;
    }
}

