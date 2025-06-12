using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("===== UI =====")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverText;

    [Header("===== 설정 =====")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private MonsterSpawner spawner;

    private bool isGameOver = false;
    private bool isGameClear = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if ((isGameOver || isGameClear) && Input.GetKeyDown(KeyCode.Q))
        {
            RestartGame();
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver || isGameClear) return;

        isGameOver = true;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverText != null) gameOverText.text = "Game Over\nPress Q to Restart";

        if (spawner != null) spawner.DestroyAllMonsters();
        if (spawner != null) spawner.gameObject.SetActive(false);
        if (waveManager != null) StopAllCoroutines();

        Debug.Log("게임 오버!");
    }

    public void TriggerGameClear()
    {
        if (isGameOver || isGameClear) return;

        isGameClear = true;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverText != null) gameOverText.text = "Game Clear!\nPress Q to Restart";

        if (spawner != null) spawner.DestroyAllMonsters();
        if (spawner != null) spawner.gameObject.SetActive(false);
        if (waveManager != null) StopAllCoroutines();

        Debug.Log("게임 클리어!");
    }

    public void RestartGame()
    {
        isGameOver = false;
        isGameClear = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (spawner != null) spawner.gameObject.SetActive(true);
        if (waveManager != null) waveManager.ResetWaves();

        Debug.Log("게임 재시작");
    }

    public bool IsGameOver() => isGameOver;
    public bool IsGameClear() => isGameClear;
}
