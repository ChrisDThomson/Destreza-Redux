using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BookInfo : MonoBehaviour
{
    static public BookCurlPro.AutoFlip autoFlip;

    static public BookCurlPro.BookPro book;

    static public PageStages currentPage = PageStages.MainMenu;

    SceneCoordinator sceneCoordinator;

    private void Start()
    {
        sceneCoordinator = FindObjectOfType<SceneCoordinator>();
    }

    static public void FlipLeft()
    {
        autoFlip.FlipLeftPage();
    }

    static public void FlipRight()
    {
        autoFlip.FlipRightPage();
    }

    static public void SetToCurrentPage()
    {
        Debug.Log("Called me" + currentPage.ToString());
        autoFlip.StartFlipping((int)currentPage);
    }

    void OnEnable()
    {
        autoFlip = GetComponent<BookCurlPro.AutoFlip>();
        book = GetComponent<BookCurlPro.BookPro>();
    }
}

[System.Serializable]
public enum PageStages
{
    MainMenu = 1,
    CharacterSelect = 2,
    Level = 3,
    WinScreen = 4,

    PageStagesCount = 4

}
