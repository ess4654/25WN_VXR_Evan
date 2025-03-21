/// <summary>
///     A card.
/// </summary>
[System.Serializable]
public class Card : CardBase
{
    public Card() : base() { }
    public Card(string code) : base(code) { }
    public Card(CardSuit suit, CardFace face) : base(suit, face) { }
    public Card(ICard card) : base(card) { }

    #region EQUALITY OPERATORS
    //card comparisons
    public static bool operator ==(Card A, ICard B) => A is Card c && c.Equals(B);
    public static bool operator !=(Card A, ICard B) => !(A is Card c && c.Equals(B));

    public static bool operator ==(ICard A, Card B) => B is Card c && c.Equals(A);
    public static bool operator !=(ICard A, Card B) => !(B is Card c && c.Equals(A));

    public static bool operator ==(Card A, Card B) => A is Card c && c.Equals(B);
    public static bool operator !=(Card A, Card B) => !(A is Card c && c.Equals(B));

    //string comparisons
    public static bool operator ==(Card card, string code) => card is Card c && c.Equals(code);
    public static bool operator !=(Card card, string code) => !(card is Card c && c.Equals(code));

    public static bool operator ==(string code, Card card) => card is Card c && c.Equals(code);
    public static bool operator !=(string code, Card card) => !(card is Card c && c.Equals(code));

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
    #endregion
}