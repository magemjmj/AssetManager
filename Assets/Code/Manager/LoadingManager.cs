using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : Singleton<LoadingManager>
{
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void LoadScene(string scenename)
    {
        StartCoroutine(LoadSceneCoroutine(scenename));
    }
    
    IEnumerator LoadSceneCoroutine(string scenename)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("loading");
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1.0f); 

        Scene loadingScene = SceneManager.GetSceneByName("loading");
        Scene tempScene = SceneManager.CreateScene("TempScene");
        SceneManager.SetActiveScene(tempScene);

        //SceneManager.LoadSceneAsync("Kidsroom");
        AssetManager.GetManager().LoadLevelAsync("scene", scenename, true, () =>
        {
            Scene newScene = SceneManager.GetSceneByName(scenename);
            SceneManager.MergeScenes(tempScene, newScene);

            SceneManager.SetActiveScene(newScene);

            StartCoroutine(LoadSceneComplete());
        });
    }    
    
    IEnumerator LoadSceneComplete()
    {
        yield return new WaitForSeconds(1.0f); 
        SceneManager.UnloadSceneAsync("loading");
    }
}
