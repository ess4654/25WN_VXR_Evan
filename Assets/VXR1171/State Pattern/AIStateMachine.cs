using Shared.Editor;
using UnityEngine;

/// <summary>
///     A state machine to handle AI movement for a combat character.
/// </summary>
public class AIStateMachine : StateMachineBase<AIStates>
{
    #region VARIABLE DECLATIONS

    protected override AIStates StartingState => AIStates.Patrolling;

    [SerializeField] private Transform player;
    [SerializeField] private float pursueRange = 8f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float stunTime = 3f;
    [SerializeField] private float fleeDistance = 10f;

    /// <summary>
    ///     Is the AI dead?
    /// </summary>
    public bool IsDead => isDead;
    [Space]
    [SerializeField, ReadOnly] private bool isDead;
    [SerializeField, ReadOnly] private float health = 100f;
    [SerializeField, ReadOnly] private float stunCountdown;
    [SerializeField, ReadOnly] private float currentFlee;
    [SerializeField, ReadOnly] private float distanceToPlayer;

    //the distance to the player
    private float DistanceToPlayer
    {
        get
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
            return distanceToPlayer;
        }
    }

    #endregion

    #region ENGINE

    protected override bool CanTransitionTo(AIStates nextState)
    {
        if (CurrentState == AIStates.Dead) return false; //dead players don't do anything
        else if (nextState == AIStates.Pusuing) //pursuing state can be set from fighting, fleeing or patrolling
        {
            if (CurrentState == AIStates.Fighting)
                return DistanceToPlayer > attackRange; //only when the player has left the attack range do we pursue
            else if (CurrentState == AIStates.Fleeing || CurrentState == AIStates.Patrolling)
                return DistanceToPlayer < pursueRange; //only when the player is within our pursuit range
            
            return false;
        }
        else if(nextState == AIStates.Patrolling)
        {
            if (CurrentState == AIStates.Fleeing)
                return DistanceToPlayer > pursueRange; //we go back to patrolling when fleeing is done and player is out of AI's detection range
            else if (CurrentState == AIStates.Fighting)
                return player.name.Contains("Dead");

            //I made a mistake in the state diagram as the player will never be outside the
            //pursue range from the fighting state as they will transition to pursue
            //before. patrolling only happens if the player is killed.
            
            return false;
        }

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
        else if (nextState == AIStates.Stunned)
        {
            requiredState = AIStates.Fighting; //we can only transition to stunned state from fighting
            if (CurrentState != requiredState) return true;
        }
        else if (nextState == AIStates.Fighting)
        {
            requiredState = AIStates.Pusuing; //we can only transition to fighting state from pursuing
            if (CurrentState != requiredState) return true;
        }

        return false;
    }

    protected override void OnStateChange(AIStates newState)
    {
        if (newState == AIStates.Stunned) //enemy has been stunned
            stunCountdown = stunTime;
        else if (newState == AIStates.Dead) //enemy has died
            isDead = true;
        else if (newState == AIStates.Fleeing)
            currentFlee = 0;
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

            case AIStates.Patrolling:
                if (DistanceToPlayer < pursueRange)
                    ChangeState(AIStates.Pusuing); //we detected the player inside of our pursue range

                break;

            case AIStates.Fleeing:
                currentFlee += Time.deltaTime; //increase the flee distance. in a real game we would base it on transform movement instead of a timer
                if(currentFlee >= fleeDistance)
                {
                    var distance = DistanceToPlayer;
                    if (distance > pursueRange)
                        ChangeState(AIStates.Patrolling);
                    else
                        ChangeState(AIStates.Pusuing);
                }
                
                break;

            case AIStates.Pusuing:
                if (DistanceToPlayer < attackRange)
                    ChangeState(AIStates.Fighting); //AI has entered the range to attack the player

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

        bool stunned = (damage / health) > 0.3f;
        health -=  damage;
        if (health <= 0)
            ChangeState(AIStates.Dead); //AI has died
        else if(stunned)
            ChangeState(AIStates.Stunned); //Player has dealt more than 30% of our remaining health in damages
    }

    #endregion

    #region DEBUGGING

    [Space]
    [Header("Debugging")]
    [SerializeField] AIStates m_state;
    [SerializeField] float m_damage;
    
    [Space]
    [SerializeField, InspectorButton("TestTakeDamage")] bool m_TakeDamage;
    [SerializeField, InspectorButton("TestSetState")] bool m_ChangeState;
    [SerializeField, InspectorButton("KillPlayer")] bool m_KillPlayer;
    
    void TestTakeDamage() => TakeDamage(m_damage);
    void TestSetState() => ChangeState(m_state);
    void KillPlayer()
    {
        player.name = "Player (Dead)";
        if (CurrentState == AIStates.Fighting)
            ChangeState(AIStates.Patrolling); //return to patrolling after killing the player
    }

    #endregion
}