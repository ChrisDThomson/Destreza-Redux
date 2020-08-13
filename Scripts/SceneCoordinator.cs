using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCoordinator : MonoBehaviour
{
    public string startingSceneName;
    static public Scene startingScene;

    public int SceneStartingIndex = 1;

    static public string CurrentSceneName;

    static public SceneCoordinator instance;
    static int oldIndex = -1;
    static int currentIndex = -1;

    static public List<Scene> scenes;
    // Start is called before the first frame update
    private void Awake()
    {
        SceneCoordinator sceneCoordinator = FindObjectOfType<SceneCoordinator>();
        if (sceneCoordinator == this && SceneCoordinator.instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        startingScene = gameObject.scene;
        startingSceneName = startingScene.name;

        //StartCoroutine(LoadYourAsyncScene());


        if (scenes == null)
            scenes = new List<Scene>();

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            SceneManager.LoadScene(i, LoadSceneMode.Additive);
            scenes.Add(SceneManager.GetSceneByBuildIndex(i));

            foreach (var item in SceneManager.GetSceneByBuildIndex(i).GetRootGameObjects())
            {
                Debug.Log("Boefsdfds");
                item.transform.position += Vector3.right * (-40.0f + (1.0f * (float)i));
                item.SetActive(false);
            }
        }

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
        }



    }

    private void Start()
    {


        BookInfo.autoFlip.onFlipFinished += OnPageFlipFinished;
        TurnOffAllScenesExceptActive(SceneStartingIndex);
    }

    IEnumerator LoadYourAsyncScene()
    {
        var thisScene = SceneManager.GetActiveScene();

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (thisScene.buildIndex == i) continue;

            if (SceneManager.GetSceneByBuildIndex(i).IsValid()) continue;

            SceneManager.LoadScene(i, LoadSceneMode.Additive);


        }

        for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (thisScene.buildIndex == i) continue;

            if (SceneManager.GetSceneByBuildIndex(i).IsValid()) continue;

            foreach (GameObject g in SceneManager.GetSceneByBuildIndex(i).GetRootGameObjects())
            {
                g.SetActive(false);
            }

        }

        yield return null;
    }

    static void TurnOffAllScenesExceptActive(int targetIndex)
    {
        foreach (Scene s in scenes)
        {
            if (s == startingScene)
                continue;

            if (oldIndex != -1)
                if (s == SceneManager.GetSceneAt(oldIndex))
                    continue;

            foreach (GameObject g in s.GetRootGameObjects())
            {
                if (g.activeSelf)
                    g.SetActive(false);

            }


        }

        foreach (GameObject g in SceneManager.GetSceneAt(targetIndex).GetRootGameObjects())
        {
            if (!g.activeSelf)
                g.SetActive(true);
        }
    }

    public void OnPageFlipFinished()
    {
        if (oldIndex != -1)
            SetSceneAtIndexActiveState(oldIndex, false);
    }

    static public void SetSceneAtIndexActiveState(int index, bool isActive = true)
    {
        foreach (GameObject g in SceneManager.GetSceneAt(index).GetRootGameObjects())
        {
            if (!g.activeSelf)
                g.SetActive(isActive);
        }
    }

    static public void SetNewScene(int index)
    {
        if (currentIndex != -1)
            oldIndex = currentIndex;

        currentIndex = index;
        TurnOffAllScenesExceptActive(index);

        CurrentSceneName = SceneManager.GetSceneAt(index).name;
        Debug.Log("Selection" + CurrentSceneName);
        PageStages page = (PageStages)index;
        BookInfo.currentPage = page;
        BookInfo.SetToCurrentPage();
    }


}
