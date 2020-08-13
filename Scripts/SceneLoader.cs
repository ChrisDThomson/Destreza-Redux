using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public UnityEvent onLoadEvent;

    public PageStages newPage;
    public void LoadScene()
    {
        if (!SceneManager.GetSceneByName(sceneName).IsValid())
        {
            if (onLoadEvent != null)
                onLoadEvent.Invoke();

            //SceneManager.LoadScene(sceneName);
            // BookInfo.currentPage;
        }
        else
            print("Wrong!");
    }

    public void LoadScene(string scene)
    {
        sceneName = scene;

        LoadScene();
    }

    public void SetNewPage(int buildIndex)
    {
        SceneCoordinator.SetNewScene(buildIndex);
    }
}
