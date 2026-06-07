using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MobTankAI : MonoBehaviour
{
    public Transform player;

    public float chaseDistance = 30f;
    public float stopDistance = 12f;
    public float moveSpeed = 4f;
    public float rotationSpeed = 90f;
    public float attackAngle = 10f;

    private Rigidbody rb;
    private TankShooter shooter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        shooter = GetComponent<TankShooter>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (shooter != null)
        {
            shooter.playerInput = false;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - rb.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (distance > chaseDistance || distance < 0.1f) return;

        Quaternion targetRotation = Quaternion.LookRotation(toPlayer);

        rb.MoveRotation(
            Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            )
        );

        if (distance > stopDistance)
        {
            Vector3 move = transform.forward * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    private void Update()
    {
        if (player == null || shooter == null) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;
        float angle = Vector3.Angle(transform.forward, toPlayer);

        if (distance <= stopDistance + 3f && angle <= attackAngle)
        {
            shooter.Fire();
        }
    }
}