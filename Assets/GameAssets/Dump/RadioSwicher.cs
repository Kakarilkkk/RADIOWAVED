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

    [Header("--------------Radio Stats----------------")]
    public int firstController;
    public int secondController;
    public int lampID;
    public int antennaID;

    [Header("--------------Radio Switcher----------------")]
    [SerializeField] bool caughtTheWave = false;
    public GameObject[] waves;


    void Start()
    {

    }


    void Update()
    {
        firstController = controller1.GetComponent<RoundController>().value;
        secondController = controller2.GetComponent<RoundController>().value;


        // metr get animator set bool true;


        switch (caughtTheWave)
        {
            case true:

                break;
            case false:

                break;
        }

        Listen();
    }

    void Listen()
    {
        foreach (GameObject wave in waves)
        {

        }
    }
}
