using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Arbiter : MonoBehaviour
{
    public GameEvent onPlayerJoined;
    PlayerInputManager inputManager;

    public PlayerInputInfo inputInfo;

    public static Arbiter instance = null;
    public Scene startingScene;
    // Start is called before the first frame update


    private void Awake()
    {
        Arbiter a = FindObjectOfType<Arbiter>();
        if (a == this && Arbiter.instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        inputInfo?.Reset();
        startingScene = SceneManager.GetActiveScene();


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        BookInfo.autoFlip.onFlipFinished += PlayerJoinedEvent;
    }

    public void PlayerJoined(int playerIndex, Player player)
    {
        inputInfo.players.Add(player);
        player.transform.parent = transform;

        onPlayerJoined.Raise();
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene == startingScene)
            return;

        PlayerJoinedEvent();
    }

    public void PlayerJoinedEvent()
    {
        foreach (var item in inputInfo.players)
        {
            onPlayerJoined.Raise();
        }
    }

    

}
