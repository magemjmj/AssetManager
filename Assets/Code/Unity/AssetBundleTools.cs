using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR	
using UnityEditor;
#endif

namespace AssetBundleTools
{
#if UNITY_EDITOR
    public class AssetBundlesMenuItems
    {
        const string kSetScriptAssetBundleName = "Tools/AssetBundles/Set Script AssetBundle Name";

        [MenuItem(kSetScriptAssetBundleName)]
        public static void SetScriptAssetBundleName()
        {
            Debug.Log("MenuItem => " + kSetScriptAssetBundleName);
            string[] assetLists = AssetUtility.GetFilesAtPath("Data/Scripts", new string[] { "lua", });

            foreach (string assetPath in assetLists)
            {
                AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant("scripts", "");
                Debug.Log(assetPath);
            }
        }
    }
#endif
}