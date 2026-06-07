using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public enum TankType
    {
        Player,
        Mob
    }

    [Header("Tank Type")]
    public TankType tankType = TankType.Mob;

    [Header("Health")]
    public int maxHP = 3;

    [Header("Effect")]
    public GameObject tankExplosionPrefab;
    public float explosionLifeTime = 3f;

    private int currentHP;
    private bool isDead = false;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;

        Debug.Log(gameObject.name + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        CreateTankExplosion();

        if (tankType == TankType.Player)
        {
            Debug.Log("Player 사망");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerDead();
            }

            gameObject.SetActive(false);
        }
        else if (tankType == TankType.Mob)
        {
            Debug.Log("Mob 사망 - Kill Count 증가");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKill();
            }

            Destroy(gameObject);
        }
    }

    private void CreateTankExplosion()
    {
        if (tankExplosionPrefab == null)
        {
            Debug.LogWarning("Tank Explosion Prefab이 연결되지 않았습니다.");
            return;
        }

        GameObject effect = Instantiate(
            tankExplosionPrefab,
            transform.position,
            Quaternion.identity
        );

        Debug.Log("TankExplosion 생성됨: " + tankExplosionPrefab.name);

        Destroy(effect, explosionLifeTime);
    }
}