/// <summary>
///     Handles exceptions where transition from a given state requires a starting state.
/// </summary>
public class RequiredStateException : System.Exception
{
    /// <summary>
    ///     The required state for transition to occur.
    /// </summary>
    public AIStates RequiredState { get; private set; }

    public RequiredStateException(AIStates state) : base($"Transition must happen from the {state} state.")
    {
        RequiredState = state;
    }
}