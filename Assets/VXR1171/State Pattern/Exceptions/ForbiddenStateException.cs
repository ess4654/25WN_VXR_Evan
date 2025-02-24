/// <summary>
///     Handles exceptions where it is forbidden to transition from state A to state B.
/// </summary>
public class ForbiddenStateException : System.Exception
{
    /// <summary>
    ///     The state attempting to transition from.
    /// </summary>
    public AIStates FromState { get; private set; }

    /// <summary>
    ///     The state attempting to transition to.
    /// </summary>
    public AIStates ToState { get; private set; }

    public ForbiddenStateException(AIStates fromState, AIStates toState) : base($"Transition from state {fromState} to {toState} is forbidden.")
    {
        FromState = fromState;
        ToState = toState;
    }
}