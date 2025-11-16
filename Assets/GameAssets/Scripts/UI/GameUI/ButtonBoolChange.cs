using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBoolChange : MonoBehaviour
{
    bool isOpened = false;
    public void SwitchBool()
    {
        isOpened = !isOpened;
        GetComponent<Animator>().SetBool("isOpened", isOpened);
    }

    public void TurnOffTheBool()
    {
        isOpened = false;
        GetComponent<Animator>().SetBool("isOpened", isOpened);
    }
}
