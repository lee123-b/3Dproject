using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount = 5;
        public float spawnInterval = 1f;
    }

    [Header("Spawner")]
    [SerializeField] private MonsterSpawner spawner;

    [Header("Wave ����")]
    [SerializeField] private List<Wave> waves;

    [Header("UI")]
    [SerializeField] private Text waveText;
    [SerializeField] private Text scoreText;

    private int currentWave = 0;
    private int remainingEnemies = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateScoreUI();
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWave < waves.Count)
        {
            Wave wave = waves[currentWave];

            remainingEnemies = wave.enemyCount;

            Debug.Log($"[WaveManager] ���̺� {currentWave + 1} ����: {wave.enemyCount}���� ��ȯ");

            UpdateWaveUI();

            yield return StartCoroutine(spawner.SpawnWave(wave.enemyCount, wave.spawnInterval));

            yield return new WaitUntil(() => remainingEnemies <= 0);

            Debug.Log($"[WaveManager] ���̺� {currentWave + 1} �Ϸ�");

            spawner.DestroyAllMonsters();
            currentWave++;

            yield return new WaitForSeconds(2f);
        }

        // ��� ���̺� �Ϸ� �� ó��
        if (waveText != null)
            waveText.text = "Wave : Clear";

        if (GameManager.Instance != null)
            GameManager.Instance.TriggerGameClear();
    }

    public void OnEnemyKilled()
    {
        remainingEnemies--;
        Debug.Log($"[WaveManager] �� ���! ���� �� ��: {remainingEnemies}");

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = $"Score: {ScoreManager.Instance.GetScore()}";
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave + 1} / {waves.Count}";
        }
    }

    public void ResetWaves()
    {
        StopAllCoroutines();
        currentWave = 0;
        spawner.ResetSpawner();
        StartCoroutine(RunWaves());
    }

    public int GetRemainingEnemies() => remainingEnemies;
}
