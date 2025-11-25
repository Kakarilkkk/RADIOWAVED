using UnityEngine.UI;
using UnityEngine;

public class Antenna : MonoBehaviour
{
    public int ID;

    public Sprite UISprite;

    void Start()
    {
        UISprite = GetComponent<Image>().sprite;
    }

    public void ChangeAntennaID()
    {
        Debug.Log(ID);
        RadioSwicher.instance.antennaID = ID;

// change sprite
        GameObject radio = RadioSwicher.instance.gameObject;
        GameObject antenna = radio.transform.Find("Antenna").gameObject;
        antenna.GetComponent<SpriteRenderer>().sprite = UISprite;
    }

    public void ChangeBulbID()
    {
        Debug.Log(ID);
        RadioSwicher.instance.lampID= ID;
        // change sprite
    }

}
