using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("무언가 닿음: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("적이 게임오버존에 진입 → 게임 오버 발생");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
            else
            {
                Debug.LogWarning("GameManager.Instance가 null입니다. 씬에 GameManager가 존재하는지 확인하세요.");
            }
        }
    }
}
