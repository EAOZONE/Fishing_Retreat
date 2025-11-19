using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayakPaddleMovement : MonoBehaviour
{
    [Header("References")]
    public Transform paddle;          // Assign paddle transform (the one being held)
    public Rigidbody kayakRigidbody;  // Assign kayak's Rigidbody

    [Header("Settings")]
    public float strokeForce = 6f;    // How strong each stroke feels
    public float damping = 0.97f;     // Water drag (lower = more drag)
    public float minEffectiveSpeed = 0.05f; // Ignore tiny paddle motions

    private Vector3 lastPaddlePos;

    void Start()
    {
        if (paddle != null)
            lastPaddlePos = paddle.position;
    }

    void FixedUpdate()
    {
        if (paddle == null || kayakRigidbody == null) return;

        // Calculate paddle velocity in world space
        Vector3 paddleVelocity = (paddle.position - lastPaddlePos) / Time.fixedDeltaTime;
        lastPaddlePos = paddle.position;

        // Project the paddle motion onto the kayak's forward axis in world space
        float strokePower = Vector3.Dot(paddleVelocity, -kayakRigidbody.transform.forward);

        // --- Diagnostic Data ---
        Vector3 forwardForce = Vector3.zero;
        float paddleSpeed = paddleVelocity.magnitude;
        float angularSpeed = kayakRigidbody.angularVelocity.magnitude;

        if (strokePower > 0.1f)
        {
            forwardForce = kayakRigidbody.transform.forward * strokePower * strokeForce;
            kayakRigidbody.AddForce(forwardForce, ForceMode.Acceleration);

            // ---------- NEW TURNING LOGIC ----------
            // Determine which side of the kayak the paddle is on
            Vector3 localPaddlePos = kayakRigidbody.transform.InverseTransformPoint(paddle.position);
            float side = Mathf.Sign(localPaddlePos.x);  // +1 = right side, -1 = left side

            // Apply a gentle torque (turn) opposite to paddle side
            float turnStrength = strokePower * 0.5f; // adjust this coefficient for wider/narrower turns
            kayakRigidbody.AddTorque(Vector3.up * side * turnStrength, ForceMode.Acceleration);
        }

        // Apply drag to smooth out kayak motion
        kayakRigidbody.velocity *= damping;

        // --- Debug Visualization ---
        Debug.DrawRay(kayakRigidbody.position, kayakRigidbody.transform.forward * 5f, Color.green, 0.1f);   // kayak forward
        Debug.DrawRay(paddle.position, paddleVelocity, Color.blue, 0.1f);                                   // paddle velocity
        Debug.DrawRay(kayakRigidbody.position, kayakRigidbody.velocity, Color.yellow, 0.1f);                // kayak velocity
        if (forwardForce != Vector3.zero)
            Debug.DrawRay(kayakRigidbody.position, forwardForce.normalized * 5f, Color.red, 0.1f);          // applied force

        if(paddleSpeed != 0.0)
        {
            // --- On-screen Debug Info ---
<<<<<<< HEAD
 /*           Debug.Log(
=======
            Debug.Log(
>>>>>>> de70214d65c24c64712e14e52245c1018d2c9f48
                $"PaddleVel: {paddleVelocity} | PaddleSpeed: {paddleSpeed:F2}\n" +
                $"StrokePower: {strokePower:F2}\n" +
                $"AppliedForce: {forwardForce}\n" +
                $"RBody Vel: {kayakRigidbody.velocity}\n" +
                $"RBody AngVel: {kayakRigidbody.angularVelocity} (mag={angularSpeed:F2})\n"
<<<<<<< HEAD
            );*/
=======
            );
>>>>>>> de70214d65c24c64712e14e52245c1018d2c9f48
        }


        /*        // Convert to kayak local space
                Vector3 localVel = transform.InverseTransformDirection(paddleVelocity);

                // Backward stroke (paddle moving toward user = negative Z)
                if (localVel.z < -minEffectiveSpeed)
                {
                    // Apply forward force to kayak
                    Vector3 forceDir = transform.forward * (-localVel.z) * strokeForce;
                    kayakRigidbody.AddForce(forceDir, ForceMode.Acceleration);

                    Debug.DrawRay(transform.position, transform.forward * 5f, Color.green, 0.1f);
                    Debug.Log($"Paddle Stroke! Force: {forceDir.magnitude:F2}");
                }*/

        // Apply drag to smooth out kayak motion
        kayakRigidbody.velocity *= damping;
    }
}

/*public class KayakPaddleMovement : MonoBehaviour
{

    public Transform paddle;          // Assign your paddle here
    public Rigidbody kayakRigidbody;  // Assign kayak's Rigidbody
    public float forceMultiplier = 5f; // Adjust to make rowing stronger
    public float damping = 0.9f;       // To simulate water resistance

    private Vector3 lastPaddlePos;

    // Start is called before the first frame update
    void Start()
    {
        if (paddle != null)
            lastPaddlePos = paddle.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
            kayakRigidbody.AddForce(transform.forward * 10f, ForceMode.Acceleration);
            Debug.Log("Force applied!");
        }
    }

    void FixedUpdate()
    {
        if (paddle == null || kayakRigidbody == null) return;

        // Calculate paddle movement in world space
        Vector3 paddleVelocity = (paddle.position - lastPaddlePos) / Time.fixedDeltaTime;
        lastPaddlePos = paddle.position;

        // Convert to kayak local space to know forward/backward motion
        Vector3 localVel = transform.InverseTransformDirection(paddleVelocity);
        



        // If paddle is moving backward relative to kayak (z < 0)
        if (localVel.z < -0.1f)
        {
            Vector3 forwardForce = transform.forward * (-localVel.z) * forceMultiplier;
            kayakRigidbody.AddForce(forwardForce, ForceMode.VelocityChange);

            //Debug.Log("Kayak velocity:" + localVel.z + " | MOVE FORWARD");
            Debug.Log("Kayak Rigidbody Velocity: " + kayakRigidbody.velocity);
            Debug.DrawRay(transform.position, transform.forward * 3, Color.green, 1f);
            Debug.Log("Applying forward force: " + forwardForce);

        }
        *//*else
        {
            Debug.Log("Kayak velocity:" + localVel.z);
        }*//*



        // Apply drag to simulate water resistance
        kayakRigidbody.velocity *= damping;
    }
}*/


