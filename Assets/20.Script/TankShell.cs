using UnityEngine;

public class TankShell : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 1;
    public float hitRadius = 2.5f;

    [Header("Life Time")]
    public float lifeTime = 5f;

    [Header("Effect")]
    public GameObject shellExplosionPrefab;
    public float explosionLifeTime = 2f;

    private GameObject owner;

    public void SetOwner(GameObject ownerTank)
    {
        owner = ownerTank;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Shell 충돌 대상: " + collision.gameObject.name);

        Vector3 explosionPosition = transform.position;

        if (collision.contacts.Length > 0)
        {
            explosionPosition = collision.contacts[0].point;
        }

        CreateShellExplosion(explosionPosition);
        ApplyAreaDamage(explosionPosition);

        Destroy(gameObject);
    }

    private void CreateShellExplosion(Vector3 position)
    {
        if (shellExplosionPrefab == null)
        {
            Debug.LogWarning("Shell Explosion Prefab이 연결되지 않았습니다.");
            return;
        }

        GameObject effect = Instantiate(
            shellExplosionPrefab,
            position,
            Quaternion.identity
        );

        Debug.Log("ShellExplosion 생성됨: " + shellExplosionPrefab.name);

        Destroy(effect, explosionLifeTime);
    }

    private void ApplyAreaDamage(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, hitRadius);

        foreach (Collider hit in hits)
        {
            TankHealth health = hit.GetComponentInParent<TankHealth>();

            if (health == null) continue;

            if (owner != null && health.transform.root == owner.transform.root)
            {
                continue;
            }

            health.TakeDamage(damage);
        }
    }
}