using UnityEngine;

/// <summary>
///     A state machine to handle AI movement for a combat character.
/// </summary>
public class AIStateMachine : StateMachineBase<AIStates>
{
    #region VARIABLE DECLATIONS

    protected override AIStates StartingState => AIStates.Patrolling;

    [SerializeField] private float health = 100f;
    [SerializeField] private float stunTime = 3f;

    /// <summary>
    ///     Is the AI dead?
    /// </summary>
    public bool IsDead => isDead;
    [SerializeField] private bool isDead;
    [SerializeField] private float stunCountdown;

    #endregion

    #region ENGINE

    protected override bool CanTransitionTo(AIStates nextState)
    {
        if(CurrentState == AIStates.Dead) return false; //dead players don't do anything

        return true;
    }

    protected override bool MustRequire(AIStates nextState, out AIStates requiredState)
    {
        requiredState = AIStates.Null;

        if(nextState == AIStates.Fleeing)
        {
            requiredState = AIStates.Stunned; //we can only transition to fleeing state from stunned
            if(CurrentState != requiredState) return true;
        }

        return false;
    }

    protected override void OnStateChange(AIStates newState)
    {
        if (newState == AIStates.Stunned) //enemy has been stunned
            stunCountdown = stunTime;
        else if (newState == AIStates.Dead) //enemy has died
            isDead = true;
    }

    #endregion

    #region UPDATES

    private void Update()
    {
        switch(CurrentState)
        {
            case AIStates.Stunned:
                stunCountdown -= Time.deltaTime;
                if (stunCountdown <= 0)
                    ChangeState(AIStates.Fleeing); //flee after we have been stunned

                break;

            default: break;
        }
    }

    #endregion

    #region METHODS

    public override void ChangeState(AIStates toState)
    {
        if (!CanTransitionTo(toState))
            throw new ForbiddenStateException(CurrentState, toState);
        if (MustRequire(toState, out AIStates requiredState))
            throw new RequiredStateException(requiredState);

        UpdateState(toState); //engine
    }

    /// <summary>
    ///     Takes damage to the enemy.
    /// </summary>
    /// <param name="damage">Amount of damage to take</param>
    public void TakeDamage(float damage)
    {
        damage = Mathf.Abs(damage);
        health -=  damage;
        if (health <= 0)
            ChangeState(AIStates.Dead);
    }

    #endregion

    #region DEBUGGING
    [SerializeField] float m_damage;
    void TestTakeDamage() => TakeDamage(m_damage);
    #endregion
}