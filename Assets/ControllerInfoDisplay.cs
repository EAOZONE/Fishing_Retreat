using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // New Input System
using TMPro;

public class ControllerInfoDisplay : MonoBehaviour
{

   /* [Header("Controllers")]
    public ActionBasedController leftController;
    public ActionBasedController rightController;*/

    public ControllerButtonInfo controllerButtonInfo;

    [Header("UI")]
    public TextMeshProUGUI leftControllerText;
    public TextMeshProUGUI rightControllerText;

    void Update()
    {
/*        if (leftController) UpdateLeftUI();
        if (rightController) UpdateRightUI();*/

        UpdateLeftUI();
        UpdateRightUI();
    }

    private void UpdateLeftUI()
    {
/*        bool primaryButton = leftPrimaryButton.action.IsPressed();
        float trigger = leftController.activateActionValue.action.ReadValue<float>();
        float grip = leftController.selectActionValue.action.ReadValue<float>();*/
        bool primaryButton = controllerButtonInfo.IsLeftPrimaryPressed();
        bool secondaryButton = controllerButtonInfo.IsLeftSecondaryPressed();
        bool trigger = controllerButtonInfo.IsLeftTriggerPressed();
        bool grip = controllerButtonInfo.IsLeftGripPressed();

        leftControllerText.text =
            $"LEFT:\n" +
            $"Primary (X): {(primaryButton ? "Pressed" : "Idle")}\n" +
            $"Secondary (Y): {(secondaryButton ? "Pressed" : "Idle")}\n" +
            $"Trigger: {(trigger ? "Pressed" : "Idle")}\n" +
            $"Grip: {(grip ? "Pressed" : "Idle")}";
    }

    private void UpdateRightUI()
    {
/*        bool primaryButton = rightPrimaryButton.action.IsPressed();
        float trigger = rightController.activateActionValue.action.ReadValue<float>();
        float grip = rightController.selectActionValue.action.ReadValue<float>();*/
        bool primaryButton = controllerButtonInfo.IsRightPrimaryPressed();
        bool secondaryButton = controllerButtonInfo.IsRightSecondaryPressed();
        bool trigger = controllerButtonInfo.IsRightTriggerPressed();
        bool grip = controllerButtonInfo.IsRightGripPressed();

        rightControllerText.text =
            $"RIGHT:\n" +
            $"Primary (A): {(primaryButton ? "Pressed" : "Idle")}\n" +
            $"Secondary (B): {(secondaryButton ? "Pressed" : "Idle")}\n" +
            $"Trigger: {(trigger ? "Pressed" : "Idle")}\n" +
            $"Grip: {(grip ? "Pressed" : "Idle")}";
    }
}
