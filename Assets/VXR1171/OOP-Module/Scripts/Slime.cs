using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float m_moveSpeed = 0.5f;

    protected override void OutsideRange() => MoveTowardsPlayer();

    private void MoveTowardsPlayer()
    {
        if (m_playerTarget == null) return;

        transform.position = Vector3.MoveTowards(transform.position, m_playerTarget.position, m_moveSpeed * Time.deltaTime);
    }

    protected override void Attack()
    {
        Debug.Log($"{Name} is attacking with {m_attackDamage} damage!");
    }
}