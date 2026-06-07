using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankPlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float rotationSpeed = 120f;

    private Rigidbody rb;
    private float moveInput;
    private float rotateInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Vertical");      // W, S
        rotateInput = Input.GetAxis("Horizontal");  // A, D
    }

    private void FixedUpdate()
    {
        Vector3 move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        Quaternion turn = Quaternion.Euler(
            0f,
            rotateInput * rotationSpeed * Time.fixedDeltaTime,
            0f
        );

        rb.MoveRotation(rb.rotation * turn);
    }
}