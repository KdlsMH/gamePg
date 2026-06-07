using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject killTextObject;

    public TMP_Text resultText;
    public TMP_Text killText;

    [Header("Game Rule")]
    public int targetKillCount = 5;

    [Header("Spawn")]
    public TankSpawnManager spawnManager;

    private int currentKillCount = 0;
    private bool isPlaying = false;
    private bool isGameEnded = false;

    public bool IsPlaying
    {
        get { return isPlaying && !isGameEnded; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowStartScreen();
    }

    private void ShowStartScreen()
    {
        Time.timeScale = 0f;

        isPlaying = false;
        isGameEnded = false;
        currentKillCount = 0;

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        if (killTextObject != null)
        {
            killTextObject.SetActive(false);
        }

        UpdateKillText();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        isPlaying = true;
        isGameEnded = false;
        currentKillCount = 0;

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        if (killTextObject != null)
        {
            killTextObject.SetActive(true);
        }

        UpdateKillText();

        if (spawnManager != null)
        {
            spawnManager.StartSpawning();
        }

        Debug.Log("게임 시작");
    }

    public void AddKill()
    {
        if (!IsPlaying) return;

        currentKillCount++;

        Debug.Log("현재 처치 수: " + currentKillCount);

        UpdateKillText();

        if (currentKillCount >= targetKillCount)
        {
            GameClear();
        }
    }

    public void PlayerDead()
    {
        if (!IsPlaying) return;

        GameOver();
    }

    private void GameClear()
    {
        isGameEnded = true;
        isPlaying = false;

        Time.timeScale = 0f;

        if (spawnManager != null)
        {
            spawnManager.StopSpawning();
        }

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.text = "GAME CLEAR";
        }

        Debug.Log("게임 클리어");
    }

    private void GameOver()
    {
        isGameEnded = true;
        isPlaying = false;

        Time.timeScale = 0f;

        if (spawnManager != null)
        {
            spawnManager.StopSpawning();
        }

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.text = "GAME OVER";
        }

        Debug.Log("게임 오버");
    }

    private void UpdateKillText()
    {
        if (killText != null)
        {
            killText.text = "Kills: " + currentKillCount + " / " + targetKillCount;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}