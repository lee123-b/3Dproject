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
        Debug.Log($"[{gameObject.name}] ü�� ����: {amount}, ���� ü��: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // ���� ���� �� ó��
        var spawner = FindObjectOfType<MonsterSpawner>();
        if (spawner != null)
        {
            spawner.OnMonsterKilled();  // ���⸦ ���ؼ��� WaveManager�� ����
        }

        Destroy(gameObject);
    }
}
