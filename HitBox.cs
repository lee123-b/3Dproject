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

    [Header("이 히트박스가 담당하는 부위")]
    public BodyPart partType = BodyPart.Body;

    private void Reset()
    {
        // Collider가 Trigger로 설정되어 있지 않다면 자동으로 설정
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }
}
