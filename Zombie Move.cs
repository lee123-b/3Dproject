using UnityEngine;

public class ZombieMove: MonoBehaviour
{
    [Header("몬스터 이동 속도 (단위: 유닛/초)")]
    public float speed = 3.0f;

    void Update()
    {
        // 매 프레임마다 로컬 Z축(앞 방향)으로 이동
        // Time.deltaTime을 곱해줘야 프레임 속도(프레임 수)와 관계없이 일정 속도로 움직임
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
