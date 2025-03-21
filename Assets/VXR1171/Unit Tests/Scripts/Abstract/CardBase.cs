using System;
using UnityEngine;

/// <summary>
///     Base class used by all Cards.
/// </summary>
[Serializable]
public abstract class CardBase : ICard
{
    [SerializeField] private string _code;

    public string Code { get =>_code; protected set =>  _code = value; }
    string ICard.SetCode { get => _code; set => _code = value; } //used by the card deck

    #region CONSTRUCTORS

    /// <summary>
    ///     Constructs an empty card with no data.
    /// </summary>
    public CardBase() { }

    /// <summary>
    ///     Constructs a new card using a code.
    /// </summary>
    /// <param name="code">Code of the card.</param>
    public CardBase(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new NullReferenceException("Card code cannot be null.");

        _code = code;
    }

    /// <summary>
    ///     Constructs a new card using a suit and face.
    /// </summary>
    /// <param name="suit">Suit of the card.</param>
    /// <param name="face">Face of the card.</param>
    public CardBase(CardSuit suit, CardFace face)
    {
        char[] faces = new char[] { 'A', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'J', 'Q', 'K', 'X' };
        _code = $"{(int)suit}{faces[(int)face]}";
    }

    /// <summary>
    ///     Copies another card.
    /// </summary>
    /// <param name="card">Card to copy.</param>
    public CardBase(ICard card)
    {
        if (card == null)
            throw new NullReferenceException("The card to copy cannot be null.");

        _code = card.Code;
    }

    #endregion

    public override string ToString() => _code;

    public virtual bool CanPlay => true;

    public void Play()
    {
        if(CanPlay)
            OnPlay(); //engine
    }

    #region ENGINE

    /// <summary>
    ///     OnPlay is called when the card play is valid and has been played.
    /// </summary>
    protected virtual void OnPlay() { }

    #endregion

    #region EQUALITY OPERATORS

    //card comparisons
    public static bool operator ==(CardBase A, ICard B) => A is CardBase c && c.Equals(B);
    public static bool operator !=(CardBase A, ICard B) => !(A is CardBase c && c.Equals(B));

    public static bool operator ==(ICard A, CardBase B) => B is CardBase c && c.Equals(A);
    public static bool operator !=(ICard A, CardBase B) => !(B is CardBase c && c.Equals(A));

    public static bool operator ==(CardBase A, CardBase B) => A is CardBase c && c.Equals(B);
    public static bool operator !=(CardBase A, CardBase B) => !(A is CardBase c && c.Equals(B));

    //string comparisons
    public static bool operator ==(CardBase card, string code) => card is CardBase c && c.Equals(code);
    public static bool operator !=(CardBase card, string code) => !(card is CardBase c && c.Equals(code));

    public static bool operator ==(string code, CardBase card) => card is CardBase c && c.Equals(code);
    public static bool operator !=(string code, CardBase card) => !(card is CardBase c && c.Equals(code));

    #endregion
}