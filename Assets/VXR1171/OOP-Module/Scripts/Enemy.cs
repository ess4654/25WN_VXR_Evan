using System.Collections;
using UnityEngine;

/// <summary>
///     Base class used by all enemies.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    #region VARIABLE DECLARATIONS

    [SerializeField] protected Transform m_playerTarget;
    [SerializeField] protected float m_attackRate = 3f;
    [SerializeField] protected float m_attackRange = 0.5f;
    [SerializeField] protected int m_attackDamage = 1;
    [SerializeField] protected float m_Health = 1f;

    /// <summary>
    ///     Is the player close enough to attack?
    /// </summary>
    protected bool IsWithinAttackRange => Vector3.Distance(transform.position, m_playerTarget.position) < m_attackRange;
    
    /// <summary>
    ///     The name of the enemy.
    /// </summary>
    [field: SerializeField] public string Name { get; protected set; }

    private Coroutine attackCoroutine;

    #endregion

    #region SETUP

    protected virtual void Awake()
    {
        if (m_playerTarget == null)
            m_playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (IsWithinAttackRange)
            WithinRange();
        else
            OutsideRange();

        if (m_Health <= 0f)
            HandleDeath();
    }

    #endregion

    #region ENGINE

    /// <summary>
    ///     Called when the player is within range of the enemy.
    /// </summary>
    protected virtual void WithinRange()
    {
        HandleAttack();
    }

    /// <summary>
    ///     Called when the player is outside of the range of the enemy.
    /// </summary>
    protected virtual void OutsideRange() { }

    /// <summary>
    ///     Handles the attack behaviour.
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    ///     Handles the death of the enemy.
    /// </summary>
    protected virtual void HandleDeath()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    
    #endregion

    #region ATTACKING

    private void HandleAttack()
    {
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        while (IsWithinAttackRange)
        {
            Attack(); //engine
            yield return new WaitForSeconds(m_attackRate);
        }

        attackCoroutine = null; // Reset the coroutine reference when the enemy moves out of range
    }

    #endregion

    /// <summary>
    ///     Takes damage reducing the enemy health.
    /// </summary>
    /// <param name="amount">Amount of health to reduce.</param>
    public void TakeDamage(float amount)
    {
        amount = Mathf.Abs(amount);
        m_Health -= amount;
    }

    #region EDITOR

    // Gizmo to draw the attack range as a green circle in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Set the gizmo color to green

        // Number of segments to approximate the circle
        int segments = 50;
        float angleStep = 360f / segments;

        // Draw the circle
        Vector3 previousPoint = transform.position + new Vector3(m_attackRange, 0f, 0f); // Start at the right side of the circle
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert angle to radians
            Vector3 newPoint = transform.position + new Vector3(Mathf.Cos(angle) * m_attackRange, 0f, Mathf.Sin(angle) * m_attackRange);
            Gizmos.DrawLine(previousPoint, newPoint); // Draw line from the previous point to the new point
            previousPoint = newPoint; // Move to the new point for the next line
        }
    }

    #endregion
}