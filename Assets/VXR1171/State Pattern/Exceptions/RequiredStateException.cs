/// <summary>
///     Handles exceptions where transition from a given state requires a starting state.
/// </summary>
public class RequiredStateException : System.Exception
{
    /// <summary>
    ///     The required state for transition to occur.
    /// </summary>
    public AIStates RequiredState { get; private set; }

    /// <summary>
    ///     The state attempting to transition to.
    /// </summary>
    public AIStates ToState { get; private set; }

    public RequiredStateException(AIStates toState, AIStates requiredState) : base($"Transition to {toState} must happen from the {requiredState} state.")
    {
        ToState = ToState;
        RequiredState = requiredState;
    }
}