using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{    
    IEnumerator Start()
    {
        yield return StartCoroutine(AssetManager.GetManager().Initialize());
    }
    
    public void NextScene()
    {
        LoadingManager.GetManager().LoadScene("Lobby_new");
    }
}