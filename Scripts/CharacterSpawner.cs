using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterSpawner : MonoBehaviour
{
    public PlayerCharacterSelectionInfo playerCharacterInfo;
    public PlayerWinInfo playerWinInfo;

    public IntCharacterDictionary characters;

    public Vector3 playerSpawnPos;

    public Character DebugCharacterP1;
    public Character DebugCharacterP2;

    int playersJoined = 0;
    PlayerInputInfo pInputInfo;

    public GameEvent newRoundStart;

    public List<Character> killedCharacters;
    public List<Character> charactersForCleanup;
    public bool waitingForMoreKilledCharacters;

    public UnityEvent onGameOverEvent;

    Dictionary<int, Queue<Character>> playerCharacters = null;

    bool didStart = false;
    //This was created to avoid race conditions and to make sure the scene  has been properly init


    // Start is called before the first frame update
    void Start()
    {
        if (characters == null)
            characters = new IntCharacterDictionary();

        if (killedCharacters == null)
            killedCharacters = new List<Character>();

        if (charactersForCleanup == null)
            charactersForCleanup = new List<Character>();

        //This is for debugging - if we start on this scene spawn players for testing
        if (Arbiter.instance.startingScene == SceneManager.GetActiveScene() && SceneManager.GetActiveScene() == gameObject.scene)
        {
            playerCharacterInfo.Player1List.Clear();
            playerCharacterInfo.Player2List.Clear();

            playerCharacterInfo.Player1List.Add(DebugCharacterP1);
            playerCharacterInfo.Player2List.Add(DebugCharacterP2);

            playerCharacterInfo.OnDebug();
        }

        if (playerCharacters == null)
        {
            playerCharacters = new Dictionary<int, Queue<Character>>();

            playerCharacters.Add(1, new Queue<Character>(playerCharacterInfo.playerCharacters[0].Reverse()));
            playerCharacters.Add(2, new Queue<Character>(playerCharacterInfo.playerCharacters[1].Reverse()));
        }

        SpawnNewCharacter(playerCharacters[1].Dequeue(), 0);
        SpawnNewCharacter(playerCharacters[2].Dequeue(), 1);

        didStart = true;
    }

    private void OnEnable()
    {
        //This can be a little confusing - this should be pooled but for now when we re-enable this object reset to a clean slate
        //This is because everything is loaded at start up to acheive the page flip effect
        if (!didStart)
            return;

        waitingForMoreKilledCharacters = false;

        foreach (Character c in charactersForCleanup)
        {
            if (c != null)
                Destroy(c.gameObject);
        }

        foreach (Queue<Character> c in playerCharacters.Values)
        {
            if (c.Count != 0)
            {
                DestroyImmediate(c.Dequeue().gameObject);
            }

        }
        foreach (Character c in characters.Values)
        {
            if (c != null)
                Destroy(c.gameObject);
        }

        playerCharacters.Clear();

        playerCharacters.Add(1, new Queue<Character>(playerCharacterInfo.playerCharacters[0].Reverse()));
        playerCharacters.Add(2, new Queue<Character>(playerCharacterInfo.playerCharacters[1].Reverse()));

        SpawnNewCharacter(playerCharacters[1].Dequeue(), 0);
        SpawnNewCharacter(playerCharacters[2].Dequeue(), 1);

        foreach (var item in characters)
        {
            characters[item.Key].BindPlayerInputs(ref pInputInfo, item.Key - 1);
            item.Value.disableInput = false;
        }
    }

    public void OnPlayerJoined(PlayerInputInfo inputInfo)
    {
        //loop through our keys
        foreach (int playerNumber in characters.Keys)
        {
            int index = playerNumber - 1;

            if (index > inputInfo.players.Count - 1)
                return;

            if (inputInfo.players.ElementAt(index))
            {
                playersJoined++;

                if (!didStart)
                {
                    if (characters.TryGetValue(playerNumber, out Character c))
                        if (c != null)
                            c.disableInput = true;
                }

                if (playersJoined == 2)
                {
                    OnAllPlayersJoined();

                }
            }
        }

        pInputInfo = inputInfo;
    }

    public void OnAllPlayersJoined()
    {
        newRoundStart.Raise();
    }

    public void OnPugnateDissapeared()
    {
        foreach (var item in characters)
        {
            characters[item.Key].BindPlayerInputs(ref pInputInfo, item.Key - 1);
            item.Value.disableInput = false;
        }
    }

    public void onCharacterKilled(int playerIndex)
    {
        // //stop listening to inputs - we are dead
        characters[playerIndex + 1].UnBindPlayerInputs(ref pInputInfo, playerIndex);
        characters[playerIndex + 1].onKilled -= onCharacterKilled;
        killedCharacters.Add(characters[playerIndex + 1]);
        charactersForCleanup.Add(characters[playerIndex + 1]);
        Debug.Log("we Are Dead");
        if (!waitingForMoreKilledCharacters)
        {
            StartCoroutine(WaitForCharacterRespawn());
        }
    }

    IEnumerator WaitForCharacterRespawn()
    {
        //Take away input
        foreach (var item in characters)
        {
            characters[item.Key].UnBindPlayerInputs(ref pInputInfo, item.Key - 1);
        }


        waitingForMoreKilledCharacters = true;
        yield return new WaitForSeconds(1.5f);

        foreach (var c in killedCharacters)
        {
            int index = c.GetPlayerIndex();
            Character character = null;

            if (playerCharacters[index + 1].Count != 0)
                character = playerCharacters[index + 1].Dequeue();

            SpawnNewCharacter(character, index);
        }

        killedCharacters.Clear();

        yield return new WaitForSeconds(0.1f);

        bool didPlayerLose = false;
        int losingPlayerNumber = 0;
        foreach (var item in characters)
        {
            if (item.Value == null)
            {
                didPlayerLose = true;

                //if we haven't set a loser yet
                if (losingPlayerNumber == 0)
                    losingPlayerNumber = item.Key;
                //tie
                else
                    losingPlayerNumber = -1;
            }
        }

        if (didPlayerLose)
        {
            int winnerIndex = 0;

            //This isn't great but if we ever need more complicated win conditions - this should change first
            if (losingPlayerNumber == 1)
                winnerIndex = 2;
            else if (losingPlayerNumber == 2)
                winnerIndex = 1;
            else if (losingPlayerNumber == -1)
                winnerIndex = 0;


            playerWinInfo.SetWinningPlayer(winnerIndex);
            onGameOverEvent.Invoke();
        }
        else
        {
            print("newRound");
            waitingForMoreKilledCharacters = false;
            newRoundStart.Raise();
        }
    }

    public void SpawnNewCharacter(Character character, int index)
    {
        if (character == null)
        {
            characters[index + 1] = null;
            return;
        }

        Character c = Instantiate(character.gameObject, transform.localPosition, Quaternion.identity, transform.parent).GetComponent<Character>();

        int playerNumber = index + 1;
        if (playerNumber % 2 != 0)
        {
            Vector3 scale = c.transform.lossyScale;
            scale.x *= -1;
            c.transform.localScale = scale;
        }

        c.onKilled += onCharacterKilled;
        characters[playerNumber] = c;

        //Place both characters
        foreach (var item in characters)
        {
            Vector3 pos = playerSpawnPos;
            if (item.Key % 2 != 0)
                pos.x *= -1;

            if (item.Value != null)
                item.Value.transform.localPosition = pos;
        }
    }

    private void OnDisable()
    {
        playerCharacterInfo.OnLeaveLevel();
    }
}

[System.Serializable]
public class IntCharacterDictionary : SerializableDictionary<int, Character> { }
