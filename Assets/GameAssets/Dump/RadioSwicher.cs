using System.Collections.Generic;
using UnityEngine;

public class RadioSwicher : MonoBehaviour
{
    public static RadioSwicher instance;
    void Awake()
    {
        instance = this;
    }


    [Header("--------------Radio GO----------------")]
    public GameObject metr;
    public GameObject controller1;
    public GameObject controller2;
    public GameObject arrow;

    [Header("--------------Radio Stats----------------")]
    public int firstController;
    public int secondController;
    public int lampID;
    public int antennaID;

    [Header("--------------Radio Switcher----------------")]
    public bool caughtTheWave = false;
    public List<GameObject> waves_In_Act1;
    public List<GameObject> waves_In_Act2;
    public List<GameObject> waves_In_Act3;
    public GameObject currentwave;
    public TimeManager clock;

    [Header("--------------UI----------------")]
    public GameObject ListenButton;

    void Start()
    {
        ListenButton.SetActive(false);

        
    }


    void Update()
    {
        firstController = controller1.GetComponent<RoundController>().value;
        secondController = controller2.GetComponent<RoundController>().value;


        // metr get animator set bool true;


        switch (caughtTheWave)
        {
            case true:
                ListenButton.SetActive(true);

                if (currentwave != null)
                ListenButton.GetComponent<DialogueTrigger>().inkJSON = currentwave.GetComponent<WaveVars>().inkJSON;
                // set audio
                break;
            case false:
                ListenButton.SetActive(false);
                ListenButton.GetComponent<DialogueTrigger>().inkJSON = null;
                // unset audio
                break;
        }

        switch (clock.act)
        {
            case 1:
            Listen(waves_In_Act1);
            break;
            case 2:
            Listen(waves_In_Act2);
            break;
            case 3:
            Listen(waves_In_Act3);
            break;
        }
        

        arrow.GetComponent<Animator>().SetBool("CaughtTheWave", caughtTheWave);
    }

void Listen(List<GameObject> wavesInAct)
{
    caughtTheWave = false;
    currentwave = null;

    foreach (GameObject wave in wavesInAct)
    {
        if (!caughtTheWave)
        {
            var vars = wave.GetComponent<WaveVars>();

            if (firstController == vars.controller1 &&
                secondController == vars.controller2 &&
                antennaID == vars.antenna &&
                lampID == vars.bulb)
            {
                caughtTheWave = true;
                currentwave = wave;
            }
        }
    }
}
}
