using UnityEngine;
using UnityEngine.Pool;

public class ShootingTower : Enemy
{
    [SerializeField] private Transform m_torret;
    [SerializeField] private Projectile m_projectilePrefab;
    
    private IObjectPool<Projectile> m_projectilePool;
    private const int poolSize = 5;

    protected override void Awake()
    {
        base.Awake();
        m_projectilePool = new ObjectPool<Projectile>(CreateBullet, OnGet, OnRelease, OnActionDestroy, maxSize: poolSize);
    }

    #region OBJECT POOLING

    private Projectile CreateBullet()
    {
        Projectile projectile = Instantiate(m_projectilePrefab, m_torret.position, m_torret.rotation);
        projectile.SetPool(m_projectilePool);

        return projectile;
    }

    private void OnGet(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);
    }

    private void OnRelease(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnActionDestroy(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    #endregion

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
        //Projectile projectile = Instantiate(m_projectilePrefab, m_torret.position, m_torret.rotation);
        var projectile = m_projectilePool.Get();
        projectile.Shoot(m_attackDamage);
    }

    #endregion
}
