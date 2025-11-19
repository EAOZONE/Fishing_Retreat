using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandAutoPaddle : MonoBehaviour
{

    public Transform leftHand;
    public Transform rightHand;
    public float followSpeed = 20f;
    public float rotateSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftHand == null || rightHand == null)
            return;

        // Position = midpoint between hands
        Vector3 targetPos = (leftHand.position + rightHand.position) * 0.5f;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        // Rotation = face direction from left → right hand
        Vector3 direction = rightHand.position - leftHand.position;
        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
    }
}
