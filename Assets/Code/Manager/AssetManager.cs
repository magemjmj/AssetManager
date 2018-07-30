using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundles;

public class AssetManager : Singleton<AssetManager>
{
    AssetBundleLoadManifestOperation m_Request = null;
    bool m_Initialized = false;

    // Use this for initialization
    IEnumerator Start()
    {
        yield return StartCoroutine(InitializeManifest());
    }

    public IEnumerator Initialize()
    {
        if (m_Request != null && m_Initialized == false)
        {
            while (m_Initialized == false)
            {
                yield return null;
            }
        }
    }

    // Initialize the downloading url and AssetBundleManifest object.
    protected IEnumerator InitializeManifest()
    {
        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project. 
        // 	Another approach would be to make this configurable in the standalone player.)
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        AssetBundleManager.SetDevelopmentAssetBundleServer();
#else
		// Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
		//AssetBundleManager.SetSourceAssetBundleURL(Application.streamingAssetsPath + "/");
		// Or customize the URL based on your deployment or configuration
		//AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
        
        //AssetBundleManager.SetSourceAssetBundleURL("http://1.235.170.142/shared/AssetBundle");
        AssetBundleManager.SetSourceAssetBundleURL(Application.streamingAssetsPath + "/");
        
        
#endif

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        m_Request = AssetBundleManager.Initialize();
        if (m_Request != null)
            yield return StartCoroutine(m_Request);

        m_Initialized = true;
    }
    /// <summary>
    /// 비동기 적으로 AssetBundle에서 Asset을로드하고 Instanciate
    /// </summary>
    public Coroutine InstantiateASync<T>(string assetBundleName, string assetName, Vector3 pos, Quaternion rot, Transform parent, Action<T> callback) where T : UnityEngine.Object
    {
        return StartCoroutine(InstantiateAsset<T>(assetBundleName, assetName, pos, rot, parent, callback));
    }

    public IEnumerator InstantiateAsset<T>(string assetBundleName, string assetName, Vector3 pos, Quaternion rot, Transform parent, Action<T> callback) where T : UnityEngine.Object
    {
        yield return StartCoroutine(LoadAsset<T>(assetBundleName, assetName, (T prefab) =>
        {
            T gameObject =  Instantiate(prefab, pos, rot, parent) as T;
            if (callback != null)
                callback(gameObject);
        }));
    }

    /// <summary>
    /// 비동기 적으로 AssetBundle에서 Asset을로드
    /// </summary>
    public Coroutine LoadAssetASync<T>(string assetBundleName, string assetName, Action<T> callback) where T : UnityEngine.Object
    {
        return StartCoroutine(LoadAsset<T>(assetBundleName, assetName, callback));
    }

    public IEnumerator LoadAsset<T>(string assetBundleName, string assetName, Action<T> callback) where T : UnityEngine.Object
    {
        var request = LoadAsset<T>(assetBundleName, assetName);

        if (request == null)
            yield break;
        yield return StartCoroutine(request);

        T asset = request.GetAsset<T>();

        if (callback != null)
            callback(asset);
    }

    public AssetBundleLoadAssetOperation LoadAsset<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
    {
        var request = AssetBundles.AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(T));
        return request;
    }

    /// <summary>
    /// 비동기 적으로 AssetBundle에서 Scene을로드
    /// </summary>
    public Coroutine LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive, Action callback)
    {
        return StartCoroutine(LoadLevel(assetBundleName, levelName, isAdditive, callback));
    }

    public IEnumerator LoadLevel(string assetBundleName, string levelName, bool isAdditive, Action callback)
    {
        var request = LoadLevelAsync(assetBundleName, levelName, isAdditive);

        if (request == null)
            yield break;
        yield return StartCoroutine(request);

        if (callback != null)
            callback();
    }

    // Load level from the given assetBundle.
    public AssetBundleLoadOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
    {
        var request = AssetBundles.AssetBundleManager.LoadLevelAsync(assetBundleName, levelName, isAdditive);
        return request;
    }
}