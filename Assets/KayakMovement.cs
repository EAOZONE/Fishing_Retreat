using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayakMovement : MonoBehaviour
{

    [Header("Paddle References")]
    public Transform leftPaddle;
    public Transform rightPaddle;

    [Header("Kayak")]
    public Rigidbody kayakRigidbody;

    [Header("Settings")]
    public float strokeForce = 6f;          // linear force
    public float turnForce = 3f;            // rotational force
    public float minSpeed = 0.05f;          // ignore tiny motions
    public float damping = 0.97f;           // water drag

    [Header("Paddle-Water Detectors")]
    public PaddleWaterDetector leftWaterDetector;
    public PaddleWaterDetector rightWaterDetector;

    // Previous frame paddle positions
    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;



    // Start is called before the first frame update
    void Start()
    {
        if (leftPaddle) lastLeftPos = leftPaddle.position;
        if (rightPaddle) lastRightPos = rightPaddle.position;
    }

    void FixedUpdate()
    {
        if (!kayakRigidbody) return;

        bool leftExists = leftPaddle != null;
        bool rightExists = rightPaddle != null;

        if (leftExists && leftWaterDetector)
            HandleSinglePaddle(leftPaddle, ref lastLeftPos, isLeft: true, leftWaterDetector);

        if (rightExists && rightWaterDetector)
            HandleSinglePaddle(rightPaddle, ref lastRightPos, isLeft: false, rightWaterDetector);

        // Water drag
        kayakRigidbody.velocity *= damping;
        kayakRigidbody.angularVelocity *= damping;
    }

    // works only when paddle is in water
    private void HandleSinglePaddle(Transform paddle, ref Vector3 lastPos, bool isLeft, PaddleWaterDetector waterDetector)
    {
        if (!waterDetector.IsInWater())
        {
            return;
        }

        // Paddle velocity
        Vector3 paddleVel = (paddle.position - lastPos) / Time.fixedDeltaTime;
        lastPos = paddle.position;

        float speed = paddleVel.magnitude;
        if (speed < minSpeed) return;

        // Backward stroke
        float strokePower = Vector3.Dot(paddleVel, -kayakRigidbody.transform.forward);
        if (strokePower <= 0f) return;

        // Forward propulsion
        Vector3 forwardForce = kayakRigidbody.transform.forward * (strokePower * strokeForce);
        kayakRigidbody.AddForce(forwardForce, ForceMode.Acceleration);

        // Turning torque
        float torqueDirection = isLeft ? +1f : -1f;
        float torqueAmount = strokePower * turnForce;
        kayakRigidbody.AddTorque(Vector3.up * torqueAmount * torqueDirection, ForceMode.Acceleration);

        // Debug visuals
        Debug.DrawRay(kayakRigidbody.position, forwardForce.normalized * 3f, Color.red, 0.1f);
        Debug.DrawRay(paddle.position, paddleVel, isLeft ? Color.cyan : Color.magenta, 0.1f);
    }

      // works when paddle is out of air and kayak moves
    /*    private void HandleSinglePaddle(Transform paddle, ref Vector3 lastPos, bool isLeft)
        {
            // Paddle velocity
            Vector3 paddleVel = (paddle.position - lastPos) / Time.fixedDeltaTime;
            lastPos = paddle.position;

            float speed = paddleVel.magnitude;
            if (speed < minSpeed) return; // too small, ignore

            // Stroke power based on backward movement (toward kayak rear)
            float strokePower = Vector3.Dot(paddleVel, -kayakRigidbody.transform.forward);
            if (strokePower <= 0f) return; // only couunt backward strokes

            // --- Forward Push ---
            Vector3 forwardForce = kayakRigidbody.transform.forward * (strokePower * strokeForce);
            kayakRigidbody.AddForce(forwardForce, ForceMode.Acceleration);

            // --- Arc Turning (Left paddle pushes right, right paddle pushes left) ---
            float torqueDirection = isLeft ? +1f : -1f;
            float torqueAmount = strokePower * turnForce;

            kayakRigidbody.AddTorque(Vector3.up * torqueAmount * torqueDirection, ForceMode.Acceleration);

            // Debug lines
            Debug.DrawRay(kayakRigidbody.position, forwardForce.normalized * 3f, Color.red, 0.1f);
            Debug.DrawRay(paddle.position, paddleVel, isLeft ? Color.cyan : Color.magenta, 0.1f);
        }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
