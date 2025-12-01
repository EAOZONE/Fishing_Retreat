using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayakMovement : MonoBehaviour
{

    [Header("Paddle References")]
    public Transform leftPaddle;
    public Transform rightPaddle;
    public PaddleWaterDetector leftDetector;
    public PaddleWaterDetector rightDetector;

    [Header("Kayak")]
    public Rigidbody kayakRigidbody;

    [Header("Thrust Settings")]
    public float baseStrokeForce = 3f; // baseline thrust
    public float depthMultiplier = 4f; // deeper = more powerful
    public float velocityMultiplier = 1.0f; // faster blade = stronger stroke
    public float thrustSmoothing = 6f; // Smoothing factor for forward motion

    [Header("Turning Settings")]
    public float turnStrength = 1.2f; // rotational force
    public float turnSmoothing = 5f; // Smoothing factor for rotation
    public float maxTurnForce = 2.5f; // Limit turning power so kayak doesn't spin too fast

    [Header("General Settings")]
    public float minStrokeSpeed = 0.04f; // ignore tiny motions
    public float damping = 0.985f; // Global water drag applied each frame
    public float maxForwardSpeed = 6f;    // Max kayak forward speed
    public float maxAngularSpeed = 2f;    // Max rotation speed (radians/sec)

    // Previous frame paddle positions
    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;

    private Vector3 smoothedThrust;
    private float smoothedTurn;



    // Start is called before the first frame update
    void Start()
    {
        if (leftPaddle) lastLeftPos = leftPaddle.position;
        if (rightPaddle) lastRightPos = rightPaddle.position;
    }

    void FixedUpdate()
    {

        // Accumulate the total force produced by both paddles
        Vector3 totalThrust = Vector3.zero;
        float totalTurn = 0f;

        // Process left paddle stroke
        if (leftPaddle)
            ApplyPaddle(leftPaddle, ref lastLeftPos, leftDetector, isLeft: true, ref totalThrust, ref totalTurn);

        // Process right paddle stroke
        if (rightPaddle)
            ApplyPaddle(rightPaddle, ref lastRightPos, rightDetector, isLeft: false, ref totalThrust, ref totalTurn);

        // Lerp makes thrust gradual and smooth, preventing jerky motion
        smoothedThrust = Vector3.Lerp(smoothedThrust, totalThrust, Time.fixedDeltaTime * thrustSmoothing);
        kayakRigidbody.AddForce(smoothedThrust, ForceMode.Acceleration);

        // Smooth turning/rotation
        smoothedTurn = Mathf.Lerp(smoothedTurn, totalTurn, Time.fixedDeltaTime * turnSmoothing);

        // Prevent extreme spinning by clamping
        float clampedTurn = Mathf.Clamp(smoothedTurn, -maxTurnForce, maxTurnForce);
        kayakRigidbody.AddTorque(Vector3.up * clampedTurn, ForceMode.Acceleration);

        // Natural drag
        kayakRigidbody.velocity *= damping;
        kayakRigidbody.angularVelocity *= damping;

        // ----- LIMIT FORWARD SPEED -----
        Vector3 horizontalVel = new Vector3(
            kayakRigidbody.velocity.x,
            0,
            kayakRigidbody.velocity.z
        );

        if (horizontalVel.magnitude > maxForwardSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxForwardSpeed;
            kayakRigidbody.velocity = new Vector3(
                horizontalVel.x,
                kayakRigidbody.velocity.y,
                horizontalVel.z
            );
        }

        // ----- LIMIT ROTATION SPEED -----
        if (kayakRigidbody.angularVelocity.magnitude > maxAngularSpeed)
        {
            kayakRigidbody.angularVelocity =
                kayakRigidbody.angularVelocity.normalized * maxAngularSpeed;
        }
    }

    // works based on paddle depth in water
    private void ApplyPaddle(Transform paddle, ref Vector3 lastPos, PaddleWaterDetector detector, bool isLeft, ref Vector3 totalThrust, ref float totalTurn)
    {
        // if paddle isn't in water (or doesn't exist), do nothing
        if (!detector || !detector.IsInWater()) return;

        // Paddle velocity
        Vector3 paddleVel = (paddle.position - lastPos) / Time.fixedDeltaTime;
        lastPos = paddle.position;

        float speed = paddleVel.magnitude;
        // if paddle speed is too slow, do nothing
        if (speed < minStrokeSpeed) return;

        // Determine backward stroke
        float strokePower = Vector3.Dot(paddleVel, -kayakRigidbody.transform.forward);

        // forward/up strokes do nothing
        if (strokePower <= 0f)
            return;

        // Stroke power from depth in water and paddle velocity
        float depthFactor = detector.depth * depthMultiplier;
        float velocityFactor = speed * velocityMultiplier;
        float power = baseStrokeForce * depthFactor * velocityFactor;

        // thrust/forward motion
        Vector3 thrust = kayakRigidbody.transform.forward * (strokePower * power);
        // Add into the total thrust sum for both paddles
        totalThrust += thrust;

        // Turning/Torque
        // Convert paddle world position → kayak-local coordinates
        Vector3 localPos = kayakRigidbody.transform.InverseTransformPoint(paddle.position);

        // How far to the side the paddle is (larger = more turning)
        float sideOffset = Mathf.Abs(localPos.x);

        // Turning power
        float turn = strokePower * power * sideOffset * turnStrength;

        // Left paddle turns right, right paddle turns left
        turn *= isLeft ? +1f : -1f;
        totalTurn += turn;

        Debug.DrawRay(paddle.position, paddleVel, isLeft ? Color.blue : Color.red, 0.1f);
    }

    // works only when paddle is in water
    /*    private void HandleSinglePaddle(Transform paddle, ref Vector3 lastPos, bool isLeft, PaddleWaterDetector waterDetector)
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
        }*/

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