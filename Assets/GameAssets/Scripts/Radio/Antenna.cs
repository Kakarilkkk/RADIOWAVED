using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : MonoBehaviour
{
    public int antennaID;
    public void log() // on click
    {
        Debug.Log(antennaID);
        RadioSwicher.instance.antennaID = antennaID;
        // change sprite
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
