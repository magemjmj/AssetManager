using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if EASY_SAVE

public partial class Utility
{
    public delegate void load_complete<T>(T parm);

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SaveToFile<T> (T parm, string filename, string tag = "") 
    {
        ES2.Save<T>(parm, filename + ".bytes" + "?tag=" + tag);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static byte[] SaveToMemory<T> (T parm, string tag = "") 
    {
        ES2Settings settings = new ES2Settings();
        settings.saveLocation = ES2Settings.SaveLocation.Memory;

        using(ES2Writer writer = ES2Writer.Create(settings))
        {
            writer.Write<T>(parm, tag);
            writer.Save();
            byte[] bytes = writer.stream.ReadAllBytes();
            return bytes;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static T LoadFromFile<T> (string filename, string tag, load_complete<T> fn) 
    {
        T load = ES2.Load<T>(filename + ".bytes" + "?tag=" + tag);
        fn (load);
        return load;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static IEnumerator LoadFromAssetBundle<T> (string assetbundle, string filename, string tag, load_complete<T> fn) 
    {
        yield return AssetManager.GetManager().StartCoroutine(AssetManager.GetManager().LoadAsset<TextAsset>(assetbundle, filename, (TextAsset prefab) => 
        {
            ES2Settings settings = new ES2Settings();
            settings.saveLocation = ES2Settings.SaveLocation.Memory;

            using( ES2Reader reader = ES2Reader.Create(prefab.bytes, settings) )
            {
                T parm = reader.Read<T>(tag);
                fn(parm);
            }
        }));
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool DeleteFile(string filename)
    {
        if (ES2.Exists (filename + ".bytes")) 
        {
            ES2.Delete (filename + ".bytes");

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif

            return true;
        } else 
        {
            return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool DeleteTag(string filename, string tag = "")
    {
        if (ES2.Exists (filename + ".bytes")) 
        {
            ES2.Delete (filename + ".bytes" + "?tag=" + tag);
            return true;
        } else 
        {
            return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool Exists(string filename)
    {
        return ES2.Exists(filename + ".bytes");
    }
}

#endif