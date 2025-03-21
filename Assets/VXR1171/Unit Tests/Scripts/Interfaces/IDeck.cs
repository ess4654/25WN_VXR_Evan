using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///     Used to define a card deck.
/// </summary>
public interface IDeck : IEnumerable<ICard>, IEnumerable
{
    //Subscribed Events
    public delegate void CardCallback(ICard card);
    event CardCallback OnDrawCard;
    public delegate void CardListCallback(IEnumerable<ICard> cards);
    event CardListCallback OnShuffledCards;

    IEnumerable<ICard> Cards { get; }
    int DeckSize { get; }
    bool IsEmpty { get; }
    ICard this[int index] { get; }

    void Init(IEnumerable<ICard> deck);
    void Init(params ICard[] deck);

    //Deck Logic
    IDeck BuildStandardDeck(bool includeJokers = false);
    void ShuffleDeck();
    ICard Draw();
    ICard Draw(int index);
    void Discard(ICard card);
    void Insert(ICard card, int index);
    void AddCardsFromDeck(ref IDeck deck, int NumberCards);
    void CombineDeck(IEnumerable<ICard> cards);
    void ClearDeck();

    //Predicates
    int Count(Predicate<ICard> predicate);
    ICard Find(Predicate<ICard> predicate);
    IEnumerable<ICard> FindAll(Predicate<ICard> predicate);

    //Has Card
    bool HasCard(string Code);
    bool HasCard(char CardType);
    bool HasCard(CardSuit suit, CardFace face);
    bool HasCard(ICard Card);

    //Indicies
    int IndexOf(string Code);
    int IndexOf(CardSuit suit, CardFace face);
    int IndexOf(ICard Card);
    int LastIndexOf(string Code);
    int LastIndexOf(CardSuit suit, CardFace face);
    int LastIndexOf(ICard Card);

    //Conversions
    List<ICard> ToList();
    List<C> ToList<C>() where C : class, ICard, new();
    ICard[] ToArray();
    C[] ToArray<C>() where C : class, ICard, new();
}