using UnityEngine;

public class TankSpawnManager : MonoBehaviour
{
    public GameObject mobPrefab;
    public Transform player;

    public int maxMobs = 1;
    public float minDistance = 5f;
    public float spawnInterval = 3f;

    private Transform[] spawnPoints;
    private float timer;
    private bool isSpawning = false;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();

        Debug.Log("SpawnPoint 개수: " + (spawnPoints.Length - 1));
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("Player 자동 연결 성공");
            }
            else
            {
                Debug.LogWarning("Player 태그를 가진 오브젝트를 찾지 못했습니다.");
            }
        }

        Debug.Log("TankSpawnManager 준비 완료");
    }

    private void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            if (CountMobs() < maxMobs)
            {
                SpawnOne();
            }
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        timer = 0f;

        Debug.Log("몬스터 스폰 시작");

        SpawnOne();
    }

    public void StopSpawning()
    {
        isSpawning = false;

        Debug.Log("몬스터 스폰 정지");
    }

    private int CountMobs()
    {
        return GameObject.FindGameObjectsWithTag("Mob").Length;
    }

    public void SpawnOne()
    {
        if (mobPrefab == null)
        {
            Debug.LogWarning("Mob Prefab이 연결되지 않았습니다.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("Player가 연결되지 않았습니다.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length <= 1)
        {
            Debug.LogWarning("SpawnPoint가 없습니다. SpawnPoint를 TankSpawnManager의 자식으로 넣으세요.");
            return;
        }

        for (int i = 0; i < 20; i++)
        {
            Transform point = spawnPoints[Random.Range(1, spawnPoints.Length)];

            float distance = Vector3.Distance(player.position, point.position);

            Debug.Log("선택된 SpawnPoint: " + point.name + " / Player와 거리: " + distance);

            if (distance >= minDistance)
            {
                GameObject mob = Instantiate(mobPrefab, point.position, point.rotation);
                mob.tag = "Mob";

                Debug.Log("Mob 생성 성공: " + point.name);
                return;
            }
        }

        Debug.LogWarning("Player와 충분히 떨어진 SpawnPoint를 찾지 못했습니다. Min Distance를 낮춰보세요.");
    }
}