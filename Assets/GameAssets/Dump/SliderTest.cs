using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderTest : MonoBehaviour
{
    // it must return an int or float based on what pos the handle is


    public Transform handle; // Assign the handle GameObject in the Inspector
    public float minValue = 0f;
    public float maxValue = 1f;
    public float trackLength = 5f; // Length of the slider track

    private float currentSliderValue;
    private Vector3 initialMousePos;
    private Vector3 initialHandlePos;

    // Public property to get the current slider value
    public float SliderValue
    {
        get { return currentSliderValue; }
    }

    void OnMouseDown()
    {
        initialMousePos = Input.mousePosition;
        initialHandlePos = handle.localPosition;
    }

    void OnMouseDrag()
    {
        Vector3 mouseDelta = Input.mousePosition - initialMousePos;
        // Project mouse movement onto the slider's axis (e.g., X-axis)
        float newHandleX = initialHandlePos.x + mouseDelta.x * 0.01f; // Adjust multiplier for sensitivity

        // Clamp the handle's position within the track bounds
        newHandleX = Mathf.Clamp(newHandleX, -trackLength / 2f, trackLength / 2f);
        handle.localPosition = new Vector3(newHandleX, handle.localPosition.y, handle.localPosition.z);

        // Calculate the slider's value based on handle position
        float normalizedPosition = (newHandleX + trackLength / 2f) / trackLength;
        currentSliderValue = Mathf.Lerp(minValue, maxValue, normalizedPosition);

        // You can raise an event here to notify other scripts about value changes
        // e.g., OnSliderValueChanged?.Invoke(currentSliderValue);
    }
}
