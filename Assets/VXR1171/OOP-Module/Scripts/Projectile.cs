using UnityEngine;
using UnityEngine.Pool;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float m_speed = 1f;  
    [SerializeField] int m_damagePower= 5;
    
    private Rigidbody m_rb;
    private IObjectPool<Projectile> registeredPool;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
        ReturnToPool();
    }

    public void Shoot(int damagePower)
    {
        m_damagePower = damagePower;

        m_rb.linearVelocity = transform.forward * m_speed;
    }

    public void SetPool(IObjectPool<Projectile> pool)
    {
        registeredPool = pool;
    }

    private void ReturnToPool()
    {
        registeredPool?.Release(this);
    }
}

