<<<<<<< HEAD
﻿using System.Collections;
=======
using System.Collections;
>>>>>>> de70214d65c24c64712e14e52245c1018d2c9f48
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils; // Needed for XROrigin
using UnityEngine.InputSystem; // New Input System

public class TeleportToKayak : MonoBehaviour
{
    [Header("References")]
    public TeleportationProvider teleportationProvider;
    public Transform kayak;                     // Assign the kayak object
    public TeleportationAnchor kayakTeleportAnchor; // Assign the kayak's seat teleport anchor
    public float heightOffset = 0.0f;           // Adjust seat height offset
    public InputAction exitKayakAction;

    private XROrigin xrOrigin;
    private bool seated = false;

    private ContinuousMoveProviderBase moveProvider;
    private SnapTurnProviderBase turnProvider;


    void Start()
    {
        xrOrigin = teleportationProvider.system.xrOrigin;

        // Cache locomotion providers if present
        moveProvider = xrOrigin.GetComponent<ContinuousMoveProviderBase>();
        turnProvider = xrOrigin.GetComponent<SnapTurnProviderBase>();
    }

    private void OnEnable()
    {
        teleportationProvider.endLocomotion += OnTeleportEnd;
        exitKayakAction.Enable();
    }

    private void OnDisable()
    {
        teleportationProvider.endLocomotion -= OnTeleportEnd;
        exitKayakAction.Disable();
    }

    private void Update()
    {
        // Optional: if seated, ensure kayak follows XR camera (for stability)
        if (seated && xrOrigin != null && kayak != null)
        {
            // Smooth follow Y-offset if desired
            Vector3 desiredPos = xrOrigin.Camera.transform.position - new Vector3(0, heightOffset, 0);
            kayak.position = Vector3.Lerp(kayak.position, desiredPos, Time.deltaTime * 3f);
        }

        // Debug: press "E" to exit kayak manually
        if (seated && exitKayakAction.triggered)
        {
            ExitKayak();
        }
    }

    private void OnTeleportEnd(LocomotionSystem locomotionSystem)
    {
        if (xrOrigin == null || kayak == null) return;

        // Check distance between XR Rig and kayak
        Vector3 playerPos = xrOrigin.transform.position;
        float distanceToKayak = Vector3.Distance(new Vector3(playerPos.x, 0, playerPos.z),
                                                 new Vector3(kayak.position.x, 0, kayak.position.z));

        if (distanceToKayak < 2.0f && !seated)
        {
            Debug.Log("Player seated in kayak.");

            // Align XR Rig height with kayak
            Vector3 desiredCameraPos = new Vector3(
                kayak.position.x,
                kayak.position.y + heightOffset,
                kayak.position.z
            );

            xrOrigin.MoveCameraToWorldLocation(desiredCameraPos);

            // Parent XR Rig to kayak
            xrOrigin.transform.SetParent(kayak, true);
            seated = true;

            // Disable teleportation + movement while seated
            if (moveProvider) moveProvider.enabled = false;
            if (turnProvider) turnProvider.enabled = false;
            if (teleportationProvider) teleportationProvider.enabled = false;
            if (kayakTeleportAnchor) kayakTeleportAnchor.enabled = false;
        }
    }

    public void ExitKayak()
    {
        if (!seated) return;

        Debug.Log("Player exiting kayak.");

        // Detach XR Rig
        xrOrigin.transform.SetParent(null, true);
        seated = false;

        // Re-enable movement + teleportation
        if (moveProvider) moveProvider.enabled = true;
        if (turnProvider) turnProvider.enabled = true;
        if (teleportationProvider) teleportationProvider.enabled = true;
        if (kayakTeleportAnchor) kayakTeleportAnchor.enabled = true;
    }
}

/*public class TeleportToKayak : MonoBehaviour
{
    public TeleportationProvider teleportationProvider;
    public Transform kayak; // Assign the kayak object here
    public TeleportationAnchor kayakTeleportAnchor; // assign your kayak seat teleport anchor here
    public float heightOffset = 0f; // Adjust if you want to sit higher/lower on the kayak
    private XROrigin xrOrigin;
    private bool seated = false;

    private void Start()
    {
        xrOrigin = teleportationProvider.system.xrOrigin;
    }

    private void Update()
    {
        if(seated && xrOrigin != null && kayak != null)
        {
            Vector3 targetPos = xrOrigin.Camera.transform.position - new Vector3(0, heightOffset, 0);
            //Vector3 targetPos = xrOrigin.Camera.transform.position;
            kayak.position = targetPos;
        }
    }

    private void OnEnable()
    {
        teleportationProvider.endLocomotion += OnTeleportEnd;
    }

    private void OnDisable()
    {
        teleportationProvider.endLocomotion -= OnTeleportEnd;
    }

    // TO DO: Figure out how to use kayak paddle to navigate
    private void OnTeleportEnd(LocomotionSystem locomotionSystem)
    {
        if (xrOrigin == null || kayak == null) return;

        // Check if player is near or inside kayak bounds after teleport
        Vector3 playerPos = xrOrigin.transform.position;
        float distanceToKayak = Vector3.Distance(new Vector3(playerPos.x, 0, playerPos.z),
                                                 new Vector3(kayak.position.x, 0, kayak.position.z));

        Debug.Log("Distance to Kayak: " + distanceToKayak);

        // You can adjust the threshold below
       // if (distanceToKayak < 2.0f)
        //{
        Debug.Log("We can bring player down now");
        Vector3 desiredCameraPos = new Vector3(
            kayak.position.x,
            kayak.position.y + heightOffset,
            kayak.position.z
        );

        // Adjust the XR Origin so that the camera moves to that height
        xrOrigin.MoveCameraToWorldLocation(desiredCameraPos);
        seated = true;

        // disable teleportation afterwards
        kayakTeleportAnchor.enabled = false;

        // Parent the kayak to XR Rig so it follows your movement
        //kayak.SetParent(xrOrigin.Camera.transform.parent);
        //}
    }
}

*/