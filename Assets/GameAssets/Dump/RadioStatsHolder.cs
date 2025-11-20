using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioStatsHolder : MonoBehaviour
{
    public GameObject controller1;
    public GameObject controller2;

    public GameObject metr;

    // Start is called before the first frame update
    public int firstController;
    public int secondController;

    public int lampID;
    public int antennaID;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        firstController = controller1.GetComponent<RoundController>().value;
        secondController = controller2.GetComponent<RoundController>().value;
    }

    // Add reference to radioSwitcher GO (switcher)
}
