using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PortraitHandler : MonoBehaviour
{
    public Portrait portrait;
    public Portrait captainPortrait;


    List<Portrait> portraits;

    public int playerNumber = -1;

    public PlayerColor playerColor;

    public Portrait activePortrait;

    Portrait oldPortrait;
    public int activePortraitIndex = 0;

    public PlayerSettingsInfo playerSettingsInfo;
    public PlayerCharacterSelectionInfo playerCharacterInfo;
    public PlayerPortraitInfo playerPortraitInfo;

    private void OnEnable()
    {
        if (portraits == null)
        {
            InitPortraits();
            TogglePortraitsVisibility(false);

            playerCharacterInfo.onCharacterPush += OnCharacterAdded;
            playerCharacterInfo.onCharacterPop += OnCharacterRemoved;

            playerPortraitInfo.InitData();

            playerPortraitInfo.onPortraitHighlighted += OnCharacterHighlighted;
        }
        else
        {
            foreach (Portrait p in portraits)
            {
                Destroy(p.gameObject);
            }

            activePortraitIndex = 0;
            InitPortraits();
            activePortrait = portraits[0];
            activePortrait.SetSwordVisiblity(false);
            TogglePortraitsVisibility(true);
        }
    }

    private void OnDisable()
    {
        // playerCharacterInfo.onCharacterPush -= OnCharacterAdded;
        // playerCharacterInfo.onCharacterPop -= OnCharacterRemoved;

        // playerPortraitInfo.onPortraitHighlighted -= OnCharacterHighlighted;
    }

    public void OnCharacterAdded(int playerIndex)
    {
        if (playerNumber - 1 == playerIndex)
        {
            if (activePortraitIndex + 1 > portraits.Count - 1)
                return;

            if (activePortraitIndex != portraits.Count - 1)
            {
                oldPortrait = portraits[activePortraitIndex];
                activePortraitIndex++;
                activePortrait = portraits[activePortraitIndex];
                activePortrait.SetSwordVisiblity(false);
            }
        }
    }

    public void OnCharacterRemoved(int playerIndex)
    {
        if (playerNumber - 1 == playerIndex)
        {
            if (activePortraitIndex - 1 < 0)
            {
                return;
            }

            if (activePortraitIndex != portraits.Count - 1)
            {
                oldPortrait = activePortrait;
                oldPortrait.SetSwordVisiblity(true);
                activePortraitIndex--;
                activePortrait = portraits[activePortraitIndex];
                activePortrait.SetSwordVisiblity(false);
                oldPortrait.characterPortrait.sprite = null;
            }
            else if (activePortrait == oldPortrait)
            {
                oldPortrait = portraits[activePortraitIndex];
                oldPortrait.SetSwordVisiblity(true);
                activePortrait.SetSwordVisiblity(false);
                oldPortrait.characterPortrait.sprite = null;
                activePortraitIndex--;
            }
            else
            {
                activePortrait = oldPortrait;
                activePortrait.SetSwordVisiblity(false);
            }

        }
    }

    public void OnCharacterHighlighted(int playerIndex)
    {
        if (playerNumber - 1 == playerIndex)
        {
            int index = playerPortraitInfo.playerPortraits[playerNumber - 1].Count - 1;

            portraits[activePortraitIndex].characterPortrait.sprite
                = playerPortraitInfo.playerPortraits[playerNumber - 1][index];
        }
    }


    public void OnPlayerJoined(PlayerInputInfo inputInfo)
    {
        int index = playerNumber - 1;
        if (index > inputInfo.players.Count - 1)
            return;


        if (inputInfo.players.ElementAt(index))
        {
            activePortrait = portraits[0];
            activePortrait.SetSwordVisiblity(false);
            TogglePortraitsVisibility(true);
        }
    }

    public void InitPortraits()
    {
        portraits = new List<Portrait>();

        for (int i = 0; i < playerSettingsInfo.StockCount; i++)
        {
            Portrait p = i != 0 ? portrait : captainPortrait;

            Portrait prefab = Instantiate(p.gameObject).GetComponent<Portrait>();

            Vector3 offset = Vector3.zero;

            if (i % 2 != 0)
            {
                offset.x = 1.28f;
                prefab.SetSwordOrientation(false);
            }
            else
            {
                prefab.SetSwordOrientation(true);
            }

            prefab.SetSwordColor(playerColor.playerColor);
            prefab.SetWindowBGColor(playerColor.windowBGColor);
            prefab.gameObject.name = i.ToString();


            offset.y = i * -1.1f;

            prefab.gameObject.transform.position = transform.position + offset;
            prefab.transform.parent = transform;


            portraits.Add(prefab);
        }

        if (playerNumber % 2 == 0)
        {
            Vector3 scale = transform.lossyScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void TogglePortraitsVisibility(bool areVisible)
    {
        foreach (Portrait p in portraits)
        {
            p.gameObject.SetActive(areVisible);
        }
    }
}
