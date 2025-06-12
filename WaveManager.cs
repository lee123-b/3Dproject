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

    [Header("Wave 설정")]
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

            Debug.Log($"[WaveManager] 웨이브 {currentWave + 1} 시작: {wave.enemyCount}마리 소환");

            UpdateWaveUI();

            yield return StartCoroutine(spawner.SpawnWave(wave.enemyCount, wave.spawnInterval));

            yield return new WaitUntil(() => remainingEnemies <= 0);

            Debug.Log($"[WaveManager] 웨이브 {currentWave + 1} 완료");

            spawner.DestroyAllMonsters();
            currentWave++;

            yield return new WaitForSeconds(2f);
        }

        // 모든 웨이브 완료 후 처리
        if (waveText != null)
            waveText.text = "Wave : Clear";

        if (GameManager.Instance != null)
            GameManager.Instance.TriggerGameClear();
    }

    public void OnEnemyKilled()
    {
        remainingEnemies--;
        Debug.Log($"[WaveManager] 적 사망! 남은 적 수: {remainingEnemies}");

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
