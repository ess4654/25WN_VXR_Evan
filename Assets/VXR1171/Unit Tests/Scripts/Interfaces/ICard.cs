/// <summary>
///     Used to define a card.
/// </summary>
public interface ICard
{
    /// <summary>
    ///     Unique code identifier of the card.
    /// </summary>
    string Code { get; }
    string SetCode { get; internal set; }

    /// <summary>
    ///     Can the card be played in the game?
    /// </summary>
    bool CanPlay { get; }

    /// <summary>
    ///     Plays the card in the game.
    /// </summary>
    void Play();
}