using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Implementation of a card deck.
/// </summary>
[Serializable]
public class CardDeck : DeckBase
{
    [SerializeField] protected string _deck;
    public override IEnumerable<ICard> Cards
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_deck)) return null;

            return _deck.Split(",").Select(code => new Card(code));
        }
        protected set
        {
            if (value != null)
                _deck = string.Join(',', value.Select(x => x.Code));
            else
                _deck = "";
        }
    }

    #region CONSTRUCTORS

    /// <summary>
    ///     Constructs a card deck.
    /// </summary>
    public CardDeck() : base() { }

    /// <summary>
    ///     Constructs a card deck given a set of cards or another deck.
    /// </summary>
    /// <param name="deck">Cards to add to the deck.</param>
    public CardDeck(IEnumerable<ICard> deck) : base(deck) { }

    /// <summary>
    ///     Constructs a card deck given a set of cards.
    /// </summary>
    /// <param name="deck">Cards to add to the deck.</param>
    public CardDeck(params ICard[] deck) : base(deck) { }

    /// <summary>
    ///     Create a deck based on a csv list of cards.
    /// </summary>
    /// <param name="deck">The deck string to copy</param>
    public CardDeck(string deck) : base(deck) { }

    #endregion
}