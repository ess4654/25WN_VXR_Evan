using UnityEngine;

public class ShootingTower : Enemy
{
    [SerializeField] private Transform m_torret;
    [SerializeField] private Projectile m_projectilePrefab;

    #region ATTACKING

    protected override void Attack()
    {
        Debug.Log($"{Name} is attacking with {m_attackDamage} damage!");

        AimTorret(m_playerTarget);
        Fire();
    }

    private void AimTorret(Transform target)
    {
        // Get the direction from the object to the player
        Vector3 direction = target.position - transform.position;

        // Zero out the Y component to prevent rotation on the vertical axis
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            // Create a rotation towards the target while ignoring vertical rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    private void Fire()
    {
        Projectile projectile = Instantiate(m_projectilePrefab, m_torret.position, m_torret.rotation);
        projectile.Shoot(m_attackDamage);
    }

    #endregion
}
