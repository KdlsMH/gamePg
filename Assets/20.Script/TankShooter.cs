using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public GameObject shellPrefab;
    public Transform firePoint;

    public float fireForce = 12f;
    public float fireCooldown = 0.5f;
    public bool playerInput = true;

    [Header("Audio")]
    public AudioClip shotFiringClip;
    public float shotVolume = 1f;

    private float nextFireTime = 0f;

    private void Update()
    {
        if (!playerInput) return;

        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    public void Fire()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (Time.time < nextFireTime) return;

        if (shellPrefab == null)
        {
            Debug.LogWarning("Shell Prefab이 연결되지 않았습니다.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint가 연결되지 않았습니다.");
            return;
        }

        nextFireTime = Time.time + fireCooldown;

        if (shotFiringClip != null)
        {
            AudioSource.PlayClipAtPoint(shotFiringClip, firePoint.position, shotVolume);
        }

        GameObject shell = Instantiate(
            shellPrefab,
            firePoint.position + firePoint.forward * 1.5f,
            firePoint.rotation
        );

        TankShell tankShell = shell.GetComponent<TankShell>();

        if (tankShell != null)
        {
            tankShell.SetOwner(gameObject);
        }

        Collider[] tankColliders = GetComponentsInChildren<Collider>();
        Collider shellCollider = shell.GetComponent<Collider>();

        if (shellCollider != null)
        {
            foreach (Collider tankCollider in tankColliders)
            {
                Physics.IgnoreCollision(tankCollider, shellCollider);
            }
        }

        Rigidbody shellRb = shell.GetComponent<Rigidbody>();

        if (shellRb == null)
        {
            Debug.LogWarning("Shell Prefab에 Rigidbody가 없습니다.");
            return;
        }

        shellRb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);

        Debug.Log("발사됨: " + gameObject.name);
    }
}