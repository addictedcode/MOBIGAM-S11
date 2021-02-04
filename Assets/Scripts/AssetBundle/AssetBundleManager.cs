using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
    #region Singleton
    public static AssetBundleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public string BundleRootPath {
        get
        {
            #if UNITY_EDITOR
                return Application.streamingAssetsPath;
            #elif UNITY_ANDROID
                return Application.persistentDataPath;
            #endif
        }
    }

    Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();

    public AssetBundle LoadBundle(string bundleName)
    {
        if (LoadedBundles.ContainsKey(bundleName))
        {
            return LoadedBundles[bundleName];
        }

        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(BundleRootPath, bundleName));

        if (bundle == null)
        {
            Debug.LogError($"{bundleName} does not exist!");
        }
        else
        {
            LoadedBundles.Add(bundleName, bundle);
        }
        return bundle;
    }

    public T GetAsset<T>(string bundleName, string asset) where T : Object
    {
        T ret = null;
        AssetBundle bundle = LoadBundle(bundleName);
        if (bundle != null)
        {
            ret = bundle.LoadAsset<T>(asset);
        }
        else
        {
            Debug.LogError($"{asset} does not exist!");
        }
        return ret;
    }

    public T[] GetAllAsset<T>(string bundleName) where T : Object
    {
        T[] ret = null;
        AssetBundle bundle = LoadBundle(bundleName);
        if (bundle != null)
        {
            ret = bundle.LoadAllAssets<T>();
        }
        else
        {
            Debug.LogError($"{bundleName} does not exist!");
        }
        return ret;
    }
}
