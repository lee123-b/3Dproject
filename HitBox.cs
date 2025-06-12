using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    public enum BodyPart
    {
        Head,
        Body,
        Hand,
        Foot
    }

    [Header("�� ��Ʈ�ڽ��� ����ϴ� ����")]
    public BodyPart partType = BodyPart.Body;

    private void Reset()
    {
        // Collider�� Trigger�� �����Ǿ� ���� �ʴٸ� �ڵ����� ����
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }
}
