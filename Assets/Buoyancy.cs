using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{

    [System.Serializable]
    public class FloatPoint
    {
        public Transform point;
        public float buoyancy = 1f;
    }

    [Header("References")]
    public FloatPoint frontFP;
    public FloatPoint backFP;
    public FloatPoint leftFP;
    public FloatPoint rightFP;
    public float waterHeight = 0f;
    public float waterDrag = 0.1f;
    public float waterAngularDrag = 0.1f;

    private Rigidbody rb;
    private FloatPoint[] floatPoints = new FloatPoint[4];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        floatPoints[0] = frontFP;
        floatPoints[1] = backFP;
        floatPoints[2] = leftFP;
        floatPoints[3] = rightFP;
    }

    void FixedUpdate()
    {
        foreach (var fp in floatPoints)
        {
            if (fp.point == null) continue;

            // how far below water surface?
            float depth = waterHeight - fp.point.position.y;

            if (depth > 0f)
            {
                // upward force proportional to depth
                rb.AddForceAtPosition(
                    Vector3.up * fp.buoyancy * depth,
                    fp.point.position,
                    ForceMode.Acceleration
                );
            }
        }

        // apply drag to simulate water
        if (transform.position.y < waterHeight + 1f)
        {
            rb.velocity *= (1f - waterDrag);
            rb.angularVelocity *= (1f - waterAngularDrag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
