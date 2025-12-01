using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class ControllerButtonInfo : MonoBehaviour
{

    [Header("Buttons")]
    public InputActionReference leftPrimaryButton;   // Left X Button
    public InputActionReference leftSecondaryButton;   // Left Y Button
    public InputActionReference rightPrimaryButton;  // Right A Button
    public InputActionReference rightSecondaryButton;  // Right B Button

    [Header("Triggers")]
    public InputActionReference leftTrigger;
    public InputActionReference rightTrigger;

    [Header("Grip")]
    public InputActionReference leftGrip;
    public InputActionReference rightGrip;

    private bool isLeftPrimaryPressed = false;
    private bool isLeftSecondaryPressed = false;
    private bool isRightPrimaryPressed = false;
    private bool isRightSecondaryPressed = false;

    private float leftTriggerValue = 0.0f;
    private float rightTriggerValue = 0.0f;

    private float leftGripValue = 0.0f;
    private float rightGripValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool IsLeftPrimaryPressed() { return isLeftPrimaryPressed; }
    public bool IsLeftSecondaryPressed() { return isLeftSecondaryPressed; }
    public bool IsRightPrimaryPressed() { return isRightPrimaryPressed; }
    public bool IsRightSecondaryPressed() { return isRightSecondaryPressed; }

    public bool IsLeftTriggerPressed() { return leftTriggerValue > 0.1f; }
    public bool IsRightTriggerPressed() { return rightTriggerValue > 0.1f; }

    public bool IsLeftGripPressed() { return leftGripValue > 0.1f; }
    public bool IsRightGripPressed() { return rightGripValue > 0.1f; }

    // Update is called once per frame
    void Update()
    {
        isLeftPrimaryPressed = leftPrimaryButton.action.IsPressed();
        isLeftSecondaryPressed = leftSecondaryButton.action.IsPressed();
        isRightPrimaryPressed = rightPrimaryButton.action.IsPressed();
        isRightSecondaryPressed = rightSecondaryButton.action.IsPressed();

        leftTriggerValue = leftTrigger.action.ReadValue<float>();
        rightTriggerValue = rightTrigger.action.ReadValue<float>();

        leftGripValue = leftGrip.action.ReadValue<float>();
        rightGripValue = rightGrip.action.ReadValue<float>();
    }
}
