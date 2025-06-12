using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"[{gameObject.name}] 체력 감소: {amount}, 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // 몬스터 삭제 및 처리
        var spawner = FindObjectOfType<MonsterSpawner>();
        if (spawner != null)
        {
            spawner.OnMonsterKilled();  // 여기를 통해서만 WaveManager에 전달
        }

        Destroy(gameObject);
    }
}
