using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("���� ����: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("���� ���ӿ������� ���� �� ���� ���� �߻�");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
            else
            {
                Debug.LogWarning("GameManager.Instance�� null�Դϴ�. ���� GameManager�� �����ϴ��� Ȯ���ϼ���.");
            }
        }
    }
}
