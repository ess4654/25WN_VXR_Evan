using System;
using UnityEngine;

/// <summary>
///     Base class used by state machines.
/// </summary>
/// <typeparam name="TState">Type of enum state being managed.</typeparam>
public abstract class StateMachineBase<TState>
where TState : Enum
{
    #region VARIABLE DECLARATIONS

    /// <summary>
    ///     Called when the state of the machine is changed.
    /// </summary>
    public static event Action<TState> OnStateChanged;

    /// <summary>
    ///     The current state of the machine.
    /// </summary>
    public TState CurrentState => currentState;
    [SerializeField] protected TState currentState;
    
    /// <summary>
    ///     The state of the machine when started.
    /// </summary>
    protected abstract TState StartingState { get; }

    #endregion

    protected virtual void Awake()
    {
        currentState = StartingState;
    }

    #region ENGINE

    /// <summary>
    ///     Are we able to transition from the <see cref="CurrentState"/> to the next?
    /// </summary>
    /// <param name="nextState">The next state attempting to transition to.</param>
    /// <returns>True if the transition can occur</returns>
    protected abstract bool CanTransitionTo(TState nextState);

    /// <summary>
    ///     Are we required to have a starting state to transition to the next?
    /// </summary>
    /// <param name="nextState">The next state attempting to transition to.</param>
    /// <param name="requiredState">(Output) The required starting state for a transition to occur.</param>
    /// <returns>True if the transition can occur</returns>
    protected abstract bool MustRequire(TState nextState, out TState requiredState);

    /// <summary>
    ///     Updates the current state of the machine.
    /// </summary>
    /// <param name="state">New <see cref="CurrentState"/> of the machine</param>
    protected void UpdateState(TState state)
    {
        currentState = state; //update the current state
        OnStateChanged?.Invoke(CurrentState); //subscribed event
        OnStateChange(CurrentState); //engine
    }

    /// <summary>
    ///     Called when the <see cref="CurrentState"/> has changed.
    /// </summary>
    /// <param name="newState">The new state of the machine.</param>
    protected virtual void OnStateChange(TState newState) { }

    #endregion

    #region METHODS

    /// <summary>
    ///     Changes the state of the machine to the provided state.
    /// </summary>
    /// <param name="toState">State to transition to.</param>
    public abstract void ChangeState(TState toState);

    #endregion
}