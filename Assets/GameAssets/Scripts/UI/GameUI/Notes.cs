using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;

public class Notes : MonoBehaviour
{
    public GameObject[] pages;
    public int currentID;

    void Update()
    {
        TurnThepage(currentID);

        currentID = Mathf.Clamp(currentID, 0, pages.Length);
    }

    public void ShowThePageInc()
    {
        currentID++;
    }

    public void ShowThePageDec()
    {
        currentID--;
    }

    public void TurnThepage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Length)
        {
            //Debug.LogWarning("Invalid page index.");
            return;
        }

        // Отключаем все объекты
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        // Включаем только выбранную страницу
        pages[pageIndex].SetActive(true);
    }

}
