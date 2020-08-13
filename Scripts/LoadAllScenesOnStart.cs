using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAllScenesOnStart : MonoBehaviour
{
    public int startingIndex = 1;
    void Awake()
    {
        for (int i = startingIndex; i < SceneManager.sceneCount; i++)
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(i).name, LoadSceneMode.Additive);
        }
    }

}
