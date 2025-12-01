using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int act = 1;

    public void NextAct()
    {
        act++;
    }

    void Update()
    {
        if(act == 3)
        {
            // go to the next day
        }

        GetComponent<Animator>().SetInteger("Act", act);
    }
}
