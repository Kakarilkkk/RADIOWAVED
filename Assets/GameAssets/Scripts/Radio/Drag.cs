using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Vector2 offset;
    void OnMouseDown()
    {
        offset = (Vector2)transform.position - MousePositionInWorld();
    }

    void OnMouseUp()
    {

    }

    void OnMouseDrag()
    {
        transform.position = MousePositionInWorld() + offset;
    }

    private Vector2 MousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
