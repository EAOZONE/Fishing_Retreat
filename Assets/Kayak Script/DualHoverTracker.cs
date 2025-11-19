using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DualHoverTracker : MonoBehaviour
{

    private bool leftHoverState = false;
    private bool rightHoverState = false;
    private bool isTrackerActive = true;

    public void SetLeftHoverState(bool state)
    {
        if(isTrackerActive) leftHoverState = state;
    }
    public void SetRightHoverState(bool state)
    {
        if (isTrackerActive) rightHoverState = state;
    }
    public void SetTrackerState(bool state)
    {
        isTrackerActive = state;
    }

    public bool IsLeftHovering() { return leftHoverState; }
    public bool IsRightHovering() { return rightHoverState; }
    public bool GetTrackerState() { return isTrackerActive; }
}
