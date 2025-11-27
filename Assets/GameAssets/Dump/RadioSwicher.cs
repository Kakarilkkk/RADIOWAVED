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
    public List<GameObject> waves;
    public GameObject currentwave;

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

        Listen();

        arrow.GetComponent<Animator>().SetBool("CaughtTheWave", caughtTheWave);
    }

    void Listen()
    {
        foreach (GameObject wave in waves)
        {
            if (
                firstController == wave.GetComponent<WaveVars>().controller1 &&
                secondController == wave.GetComponent<WaveVars>().controller2 &&
                antennaID == wave.GetComponent<WaveVars>().antenna &&
                lampID == wave.GetComponent<WaveVars>().bulb
                )
            {
                caughtTheWave = true;
                currentwave = wave;
            }
            else 
            {
                caughtTheWave = false;
                currentwave = null;
            }

        }
    }
}
