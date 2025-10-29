using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class XRFishingRod : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Transform rodTip;
    public GameObject lurePrefab;
    public LineRenderer lineRenderer;

    public float rodPower;
    public float reelSpeed = 5f;
    public float despawnDistance = 0.2f;

    private GameObject currentLure;
    private XRBaseControllerInteractor controllerInteractor;

    private bool isButtonHeld = false;
    private Vector3 offscreen = new Vector3(999, 999, 999);

    // --- velocity tracking ---
    private Vector3 lastTipPosition;
    private Vector3 tipVelocity;

    private Queue<Vector3> velocitySamples = new Queue<Vector3>();
    public int sampleCount = 6;

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.activated.AddListener(OnButtonPressed);
        grabInteractable.deactivated.AddListener(OnButtonReleased);
        grabInteractable.deactivated.AddListener(OnCastRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.activated.RemoveListener(OnButtonPressed);
        grabInteractable.deactivated.RemoveListener(OnButtonReleased);
        grabInteractable.deactivated.RemoveListener(OnCastRelease);
    }

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, offscreen);
        lineRenderer.SetPosition(1, offscreen);

        lastTipPosition = rodTip.position;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
    }

    private void OnButtonPressed(ActivateEventArgs args)
    {
        if (currentLure != null)
            isButtonHeld = true;
    }

    private void OnButtonReleased(DeactivateEventArgs args)
    {
        isButtonHeld = false;
    }

    private void OnCastRelease(DeactivateEventArgs args)
    {  
        if(currentLure) { return; }
        currentLure = Instantiate(lurePrefab, rodTip.position, rodTip.rotation);
        Rigidbody rb = currentLure.GetComponent<Rigidbody>();

        if (rb == null) rb = currentLure.AddComponent<Rigidbody>();

        // --- Use smoothed tip velocity ---
        rb.velocity = tipVelocity * rodPower;

        lineRenderer.SetPosition(0, rodTip.position);
        lineRenderer.SetPosition(1, currentLure.transform.position);
    }

    void LateUpdate()
    {
        // Raw frame velocity
        Vector3 frameVelocity = (rodTip.position - lastTipPosition) / Time.deltaTime;
        lastTipPosition = rodTip.position;

        // push sample
        velocitySamples.Enqueue(frameVelocity);

        if (velocitySamples.Count > sampleCount)
            velocitySamples.Dequeue();

        // compute smoothed average
        Vector3 sum = Vector3.zero;
        foreach (var v in velocitySamples)
            sum += v;

        tipVelocity = sum / velocitySamples.Count;

        // --- REELING ---
        if (currentLure != null && isButtonHeld)
        {
            currentLure.transform.position = Vector3.MoveTowards(
                currentLure.transform.position,
                rodTip.position,
                reelSpeed * Time.deltaTime
            );

            // If close enough, despawn
            if (Vector3.Distance(currentLure.transform.position, rodTip.position) < despawnDistance)
            {
                Destroy(currentLure);
                isButtonHeld = false;
            }
        }

        // Update line
        if (currentLure)
        {
            lineRenderer.SetPosition(0, rodTip.position);
            lineRenderer.SetPosition(1, currentLure.transform.position);
        }
        else
        {
            lineRenderer.SetPosition(0, offscreen);
            lineRenderer.SetPosition(1, offscreen);
        }
    }
}
