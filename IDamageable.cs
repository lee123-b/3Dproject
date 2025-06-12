public interface IDamageable
{
    /// <summary>
    /// 데미지를 받아 처리하는 메서드
    /// </summary>
    /// <param name="amount">입력된 데미지 양</param>
    void TakeDamage(float amount);
}
