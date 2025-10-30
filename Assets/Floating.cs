using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatUpPhysics : MonoBehaviour
{
    public float waterLevel = 0f;
    public float floatStrength = 10f;
    public float damping = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float difference = transform.position.y - waterLevel;

        if (difference < 0f)
        {
            rb.AddForce(Vector3.up * floatStrength * Mathf.Abs(difference) - rb.velocity * damping);
        }
    }
}
