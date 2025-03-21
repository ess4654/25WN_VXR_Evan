using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///     Base class used by all card decks.
/// </summary>
[Serializable]
public abstract class DeckBase : IDeck
{
    #region VARIABLE DECLARATIONS
        
    //Subscribed Events
        
    /// <summary>
    ///     Called when a card from this deck has been drawn
    /// </summary>
    public event IDeck.CardCallback OnDrawCard;
        
    /// <summary>
    ///     Called when the cards in the deck are shuffled.
    /// </summary>
    public event IDeck.CardListCallback OnShuffledCards;

    /// <summary>
    ///     The cards existing in this deck.
    /// </summary>
    public abstract IEnumerable<ICard> Cards { get; protected set; }

    /// <summary>
    ///     The current size of the card deck.
    /// </summary>
    public int DeckSize => Cards != null ? Cards.Count() : 0;

    /// <summary>
    ///     Is the deck empty (or null)?
    /// </summary>
    public bool IsEmpty => Cards == null || DeckSize == 0;

    /// <summary>
    ///     Returns a card at the given index.
    /// </summary>
    /// <param name="index">Index of the card to get.</param>
    /// <returns>Card from the deck at the given index.</returns>
    public ICard this[int index]
    {
        get
        {
            if (IsEmpty || index < 0 || index >= DeckSize)
                throw new IndexOutOfRangeException();

            return Cards.ElementAt(index);
        }
    }

    public IEnumerator GetEnumerator() => Cards == null ? new ICard[0].GetEnumerator() : Cards.GetEnumerator();
    IEnumerator<ICard> IEnumerable<ICard>.GetEnumerator() => Cards == null ? new List<ICard>().GetEnumerator() : Cards.GetEnumerator();

    #endregion

    #region SETUP

    //Constructors

    /// <summary>
    ///     Constructs an empty deck.
    /// </summary>
    public DeckBase() { }

    /// <summary>
    ///     Initialized the deck with a list of cards.
    /// </summary>
    /// <param name="deck">Deck to copy.</param>
    public void Init(IEnumerable<ICard> deck) => Cards = deck;
    public DeckBase(IEnumerable<ICard> deck) => Init(deck);

    /// <summary>
    ///     Initialized the deck with a list of cards.
    /// </summary>
    /// <param name="deck">Card list to copy.</param>
    public void Init(params ICard[] deck) => Cards = deck;
    public DeckBase(params ICard[] deck) => Init(deck);

    /// <summary>
    ///     Create a deck based on a csv list of cards.
    /// </summary>
    /// <param name="deck">The deck string to copy</param>
    public DeckBase(string deck)
    {
        if (string.IsNullOrWhiteSpace(deck)) return;

        Cards = deck.Split(",").Select(code => new Card(code));
    }

    /// <summary>
    ///     Creates a standard deck of cards.
    /// </summary>
    /// <param name="includeJokers">Includes the jokers in the deck.</param>
    /// <returns>Reference to the card deck</returns>
    public IDeck BuildStandardDeck(bool includeJokers = false)
    {
        //Create the deck
        var deck = new List<ICard>
        {
            new Card(CardSuit.Hearts, CardFace.Ace), new Card(CardSuit.Diamonds, CardFace.Ace), new Card(CardSuit.Spades, CardFace.Ace), new Card(CardSuit.Clubs, CardFace.Ace), //Ace
            new Card(CardSuit.Hearts, CardFace.Two), new Card(CardSuit.Diamonds, CardFace.Two), new Card(CardSuit.Spades, CardFace.Two), new Card(CardSuit.Clubs, CardFace.Two), //Two
            new Card(CardSuit.Hearts, CardFace.Three), new Card(CardSuit.Diamonds, CardFace.Three), new Card(CardSuit.Spades, CardFace.Three), new Card(CardSuit.Clubs, CardFace.Three), //Three
            new Card(CardSuit.Hearts, CardFace.Four), new Card(CardSuit.Diamonds, CardFace.Four), new Card(CardSuit.Spades, CardFace.Four), new Card(CardSuit.Clubs, CardFace.Four), //Four
            new Card(CardSuit.Hearts, CardFace.Five), new Card(CardSuit.Diamonds, CardFace.Five), new Card(CardSuit.Spades, CardFace.Five), new Card(CardSuit.Clubs, CardFace.Five), //Five
            new Card(CardSuit.Hearts, CardFace.Six), new Card(CardSuit.Diamonds, CardFace.Six), new Card(CardSuit.Spades, CardFace.Six), new Card(CardSuit.Clubs, CardFace.Six), //Six
            new Card(CardSuit.Hearts, CardFace.Seven), new Card(CardSuit.Diamonds, CardFace.Seven), new Card(CardSuit.Spades, CardFace.Seven), new Card(CardSuit.Clubs, CardFace.Seven), //Seven
            new Card(CardSuit.Hearts, CardFace.Eight), new Card(CardSuit.Diamonds, CardFace.Eight), new Card(CardSuit.Spades, CardFace.Eight), new Card(CardSuit.Clubs, CardFace.Eight), //Eight
            new Card(CardSuit.Hearts, CardFace.Nine), new Card(CardSuit.Diamonds, CardFace.Nine), new Card(CardSuit.Spades, CardFace.Nine), new Card(CardSuit.Clubs, CardFace.Nine), //Nine
            new Card(CardSuit.Hearts, CardFace.Ten), new Card(CardSuit.Diamonds, CardFace.Ten), new Card(CardSuit.Spades, CardFace.Ten), new Card(CardSuit.Clubs, CardFace.Ten), //Ten
            new Card(CardSuit.Hearts, CardFace.Jack), new Card(CardSuit.Diamonds, CardFace.Jack), new Card(CardSuit.Spades, CardFace.Jack), new Card(CardSuit.Clubs, CardFace.Jack), //Jack
            new Card(CardSuit.Hearts, CardFace.Queen), new Card(CardSuit.Diamonds, CardFace.Queen), new Card(CardSuit.Spades, CardFace.Queen), new Card(CardSuit.Clubs, CardFace.Queen), //Queen
            new Card(CardSuit.Hearts, CardFace.King), new Card(CardSuit.Diamonds, CardFace.King), new Card(CardSuit.Spades, CardFace.King), new Card(CardSuit.Clubs, CardFace.King) //King
        };

        //Add the jokers
        if(includeJokers)
        {
            deck.Add(new Card(CardSuit.Hearts, CardFace.Joker));
            deck.Add(new Card(CardSuit.Diamonds, CardFace.Joker));
            deck.Add(new Card(CardSuit.Spades, CardFace.Joker));
            deck.Add(new Card(CardSuit.Clubs, CardFace.Joker));
        }

        Cards = deck;
        ShuffleDeck();
        return this;
    }

    #endregion

    #region DECK LOGIC

    /// <summary>
    ///     Shuffles the card deck.
    /// </summary>
    public void ShuffleDeck()
    {
        Cards = Cards.Shuffle();
        OnDeckShuffled(); //engine
        OnShuffledCards?.Invoke(Cards); //subscribed event
    }

    /// <summary>
    ///     Draws a card from the top of the deck removing it.
    /// </summary>
    /// <returns>A drawn card</returns>
    public ICard Draw() => Draw(0);

    /// <summary>
    ///     Draws a card from a given position in the deck removing it.
    /// </summary>
    /// <param name="index">Index of the card to draw.</param>
    /// <returns>A drawn card</returns>
    public ICard Draw(int index)
    {
        if (IsEmpty)
            throw new Exception("Unable to Draw Card From Empty Deck.");

        var card = this[index];
        var list = ToList();
        list.RemoveAt(index);
        Cards = list;

        OnDrawn(card); //engine
        OnDrawCard?.Invoke(card); //subscribed event

        return card;
    }

    /// <summary>
    ///     Discards a card by adding it to the end (bottom) of the deck.
    /// </summary>
    /// <param name="card">Card to discard.</param>
    public void Discard(ICard card)
    {
        if (card == null)
            throw new NullReferenceException("card");

        if(Cards == null)
            Cards = new List<ICard> { card };
        else
            Cards = Cards.Append(card);

        OnDiscarded(card); //engine
    }

    /// <summary>
    ///     Inserts a card into the deck at a given position.
    /// </summary>
    /// <param name="card">Card to insert into the deck.</param>
    /// <param name="index">Index to insert the card.</param>
    public virtual void Insert(ICard card, int index)
    {
        if (Cards == null)
            throw new NullReferenceException("card");
        if (index < 0 || index >= DeckSize)
            throw new IndexOutOfRangeException("index");

        var list = ToList();
        list.Insert(index, card);
        Cards = list;
    }

    /// <summary>
    ///     Adds cards from another deck into the bottom if this deck.
    /// </summary>
    /// <param name="deck">Deck to draw from.</param>
    /// <param name="numberCards">The number of card to draw from the other deck.</param>
    public void AddCardsFromDeck(ref IDeck deck, int numberCards)
    {
        if (deck == null)
            throw new NullReferenceException();
        if (deck.IsEmpty || numberCards > deck.DeckSize)
            throw new Exception("Unable to draw more cards then are in the deck.");

        for (var i = 0; i < numberCards; i++)
            Discard(deck.Draw());
    }

    /// <summary>
    ///     Combines another list of cards into this deck.
    /// </summary>
    /// <param name="cards">List of cards to combine.</param>
    public void CombineDeck(IEnumerable<ICard> cards)
    {
        if (cards == null) return;
        Cards = Cards.Union(cards);
    }

    /// <summary>
    ///     Clears the deck of all cards.
    /// </summary>
    public void ClearDeck() => Cards = new ICard[] { };

    #endregion

    #region PREDICATES

    /// <summary>
    ///     Counts the number of cards in the deck given a predicate.
    /// </summary>
    /// <param name="predicate">The predicate to use for the search.</param>
    /// <returns>The number of cards found matching the predicate</returns>
    public int Count(Predicate<ICard> predicate) => predicate == null ? 0 : Cards.Count(predicate.ToFunc());

    /// <summary>
    ///     Finds the first instance of a card matching the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to use for the search.</param>
    /// <returns>The card if found</returns>
    public ICard Find(Predicate<ICard> predicate) => predicate == null ? null : ToList().Find(predicate);

    /// <summary>
    ///     Finds all instances of cards matching the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to use for the search.</param>
    /// <returns>A list of cards found</returns>
    public IEnumerable<ICard> FindAll(Predicate<ICard> predicate) => predicate == null ? null : ToList().FindAll(predicate);

    #endregion

    #region HAS CARD

    /// <summary>
    ///     Does the deck contain a card with the given code?
    /// </summary>
    /// <param name="code"></param>
    /// <returns>True if the card is in the deck</returns>
    public bool HasCard(string code) => Cards != null && Cards.Count(x => x.Code == code) > 0;

    /// <summary>
    ///     Does the deck contain a card with a given first character in its code?
    /// </summary>
    /// <param name="cardType">First character of the card code to find.</param>
    /// <returns>True if the card is in the deck</returns>
    public bool HasCard(char cardType) => Cards != null && Cards.Count(x => x.Code[0] == cardType) > 0;

    /// <summary>
    ///     Does the deck contain a card with the given suit and face?
    /// </summary>
    /// <param name="suit">Suit of the card to find.</param>
    /// <param name="face">Face of the card to find.</param>
    /// <returns>True if the card is in the deck</returns>
    public bool HasCard(CardSuit suit, CardFace face) => Cards != null && Cards.Count(x => x.Code == new Card(suit, face).Code) > 0;

    /// <summary>
    ///     Does the deck contain a given card?
    /// </summary>
    /// <param name="Card">Card to find.</param>
    /// <returns>True if the card is in the deck</returns>
    public bool HasCard(ICard Card) => Cards != null && Cards.Contains(Card);

    #endregion

    #region INDICES

    /// <summary>
    ///     The first found index of a card with the given code.
    /// </summary>
    /// <param name="code">Code of the card to find.</param>
    /// <returns>Index of the card</returns>
    public int IndexOf(string code) => Cards == null ? -1 : ToList().FindIndex(x => x.Code == code);

    /// <summary>
    ///     The first found index of a card with the given suit and face.
    /// </summary>
    /// <param name="suit">Suit of the card to find.</param>
    /// <param name="face">Face of the card to find.</param>
    /// <returns>Index of the card</returns>
    public int IndexOf(CardSuit suit, CardFace face) => IndexOf(new Card(suit, face).Code);

    /// <summary>
    ///     The first found index of a card.
    /// </summary>
    /// <param name="card">Card to find.</param>
    /// <returns>Index of the card</returns>
    public int IndexOf(ICard card) => card == null ? -1 : IndexOf(card.Code);

    /// <summary>
    ///     Gets the last index of a card with a given code.
    /// </summary>
    /// <param name="code">Code of the card to find.</param>
    /// <returns>Index of the card</returns>
    public int LastIndexOf(string code) => Cards == null ? -1 : ToList().FindLastIndex(x => x.Code == code);

    /// <summary>
    ///     Gets the last index of a card with a given suit and face.
    /// </summary>
    /// <param name="suit">Suit of the card to find.</param>
    /// <param name="face">Face of the card to find.</param>
    /// <returns>Index of the card</returns>
    public int LastIndexOf(CardSuit suit, CardFace face) => LastIndexOf(new Card(suit, face).Code);

    /// <summary>
    ///     Gets the last index of a card.
    /// </summary>
    /// <param name="card">Card to find.</param>
    /// <returns>Index of the card</returns>
    public int LastIndexOf(ICard card) => card == null ? -1 : LastIndexOf(card.Code);

    #endregion

    #region CONVERSIONS

    /// <summary>
    ///     Returns the deck as a list of ICards.
    /// </summary>
    /// <returns>Deck as list</returns>
    public List<ICard> ToList() => Cards?.ToList();
        
    /// <summary>
    ///     Returns the deck as a list of Card types.
    /// </summary>
    /// <typeparam name="TCard">Type of <see cref="ICard"/>.</typeparam>
    /// <returns>Deck as list</returns>
    public List<TCard> ToList<TCard>() where TCard : class, ICard, new()
    {
        if (DeckSize == 0) return null;

        return Cards.Select(x =>
        {
            var card = new TCard();
            card.SetCode = x.Code;

            return card;
        }).ToList();
    }

    /// <summary>
    ///     Returns the deck as an array of ICards.
    /// </summary>
    /// <returns>Deck as array</returns>
    public ICard[] ToArray() => Cards?.ToArray();

    /// <summary>
    ///     Returns the deck as an array of Card types.
    /// </summary>
    /// <typeparam name="TCard">Type of <see cref="ICard"/>.</typeparam>
    /// <returns>Deck as array</returns>
    public TCard[] ToArray<TCard>() where TCard : class, ICard, new()
    {
        if (DeckSize == 0) return null;

        return Cards.Select(x =>
        {
            var card = new TCard();
            card.SetCode = x.Code;

            return card;
        }).ToArray();
    }

    #endregion

    #region ENGINE

    /// <summary>
    ///     OnDeckShuffled is called when the deck is shuffled.
    /// </summary>
    protected virtual void OnDeckShuffled() { }

    /// <summary>
    ///     OnDiscarded is called when a card is discarded.
    /// </summary>
    /// <param name="card">Card that was discarded.</param>
    protected virtual void OnDiscarded(ICard card) { }

    /// <summary>
    ///     OnDrawn is called when a card is drawn from the deck.
    /// </summary>
    /// <param name="card">Card that was drawn.</param>
    protected virtual void OnDrawn(ICard card) { }

    #endregion

    public override string ToString()
    {
        string deck = "";
        if (Cards == null) return deck;

        deck = string.Join(",", Cards.Select(x => x.Code));

        return deck;
    }
}