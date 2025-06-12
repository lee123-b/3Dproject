using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;

    [SerializeField] private Text scoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log($"[ScoreManager] 점수 증가: {score}");
        UpdateUI();
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
