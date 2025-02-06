using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody m_rb;
 
    [SerializeField] float m_speed = 1f;  
    [SerializeField] int m_damagePower= 5;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity =false;
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);  
    }
    public void Shoot(int damagePower)
    {
        m_damagePower = damagePower;

        m_rb.linearVelocity = transform.forward * m_speed;
    }
}

