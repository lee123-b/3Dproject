using UnityEngine;

public class ZombieMove: MonoBehaviour
{
    [Header("���� �̵� �ӵ� (����: ����/��)")]
    public float speed = 3.0f;

    void Update()
    {
        // �� �����Ӹ��� ���� Z��(�� ����)���� �̵�
        // Time.deltaTime�� ������� ������ �ӵ�(������ ��)�� ������� ���� �ӵ��� ������
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
