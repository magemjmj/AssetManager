using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : Singleton<GlobalManager>
{
    bool m_Initialized = false;

    IEnumerator Start()
    {
        yield return StartCoroutine(AssetManager.GetManager().Initialize());
        m_Initialized = true;
    }

    public IEnumerator Initialize()
    {
        while (m_Initialized == false)
        {
            yield return null;
        }
    }

    void OnLevelWasLoaded(int level)
    {
#if UNITY_EDITOR && UNITY_5_3_OR_NEWER
        if (UnityEditor.Lightmapping.giWorkflowMode == UnityEditor.Lightmapping.GIWorkflowMode.Iterative)
        {
            DynamicGI.UpdateEnvironment();
        }
#endif
    }
}
