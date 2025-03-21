using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Tests for <see cref="DeckBase"/>
/// </summary>
[Category("Cards")]
public class CardDeckTests
{
    private IDeck deck = new CardDeck();
    private int expectedDeckSize;
    private bool eventCalled;
    private ICard firstCard, lastCard;

    #region NULL TESTS

    [Test]
    public void TestNull()
    {
        ICard[] cards = null;
        deck.Init(cards);
        Assert.AreEqual(0, deck.DeckSize);
        Assert.IsNull(deck.Cards);

        IEnumerable<ICard> enumerableCards = null;
        deck.Init(enumerableCards);
        Assert.AreEqual(0, deck.DeckSize);
        Assert.IsNull(deck.Cards);

        deck.BuildStandardDeck();
        Assert.IsNotNull(deck.Cards);

        string cardString = null;
        deck = new CardDeck(cardString);
        Assert.AreEqual(0, deck.DeckSize);
        Assert.IsNull(deck.Cards);

        ICard nullCard = null;
        Assert.Throws<NullReferenceException>(() =>
        {
            deck.Discard(nullCard);
        });
        Assert.Throws<NullReferenceException>(() =>
        {
            deck.Insert(nullCard, 0);
        });

        IDeck nullDeck = null;
        Assert.Throws<NullReferenceException>(() =>
        {
            deck.AddCardsFromDeck(ref nullDeck, 1);
        });

        Assert.DoesNotThrow(() =>
        {
            deck.CombineDeck(nullDeck);
            Assert.AreEqual(0, deck.Count(null));
            deck.Find(null);
            deck.FindAll(null);
            Assert.IsFalse(deck.HasCard(cardString));
            Assert.IsFalse(deck.HasCard(nullCard));
            Assert.AreEqual(-1, deck.IndexOf(cardString));
            Assert.AreEqual(-1, deck.IndexOf(nullCard));
            Assert.AreEqual(-1, deck.LastIndexOf(cardString));
            Assert.AreEqual(-1, deck.LastIndexOf(nullCard));
        });

        Assert.IsNull(deck.ToList());
        Assert.IsNull(deck.ToList<Card>());
        Assert.IsNull(deck.ToArray());
        Assert.IsNull(deck.ToArray<Card>());
    }

    #endregion

    [Test]
    public void TestConstructors()
    {
        deck = new CardDeck();
        Assert.IsTrue(deck.IsEmpty);

        deck = new CardDeck("A,B,C,D");
        Assert.AreEqual(4, deck.DeckSize);

        var copyDeck = new CardDeck(deck);
        Assert.AreEqual(deck, copyDeck);

        deck = new CardDeck(new Card("A"), new Card("B"));
        Assert.AreEqual(2, deck.DeckSize);
    }

    [Test]
    public void TestStandardDeck()
    {
        deck.BuildStandardDeck(); //no jokers
        Assert.IsFalse(deck.IsEmpty);
        Assert.AreEqual(52, deck.DeckSize);

        deck.BuildStandardDeck(true); //include jokers
        Assert.IsFalse(deck.IsEmpty);
        Assert.AreEqual(56, deck.DeckSize);

        //hearts
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Ace));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Two));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Three));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Four));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Five));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Six));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Seven));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Eight));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Nine));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Ten));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Jack));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Queen));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.King));
        Assert.IsTrue(deck.HasCard(CardSuit.Hearts, CardFace.Joker));

        //spades
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Ace));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Two));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Three));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Four));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Five));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Six));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Seven));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Eight));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Nine));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Ten));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Jack));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Queen));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.King));
        Assert.IsTrue(deck.HasCard(CardSuit.Spades, CardFace.Joker));

        //diamonds
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Ace));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Two));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Three));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Four));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Five));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Six));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Seven));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Eight));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Nine));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Ten));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Jack));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Queen));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.King));
        Assert.IsTrue(deck.HasCard(CardSuit.Diamonds, CardFace.Joker));

        //clubs
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Ace));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Two));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Three));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Four));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Five));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Six));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Seven));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Eight));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Nine));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Ten));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Jack));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Queen));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.King));
        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Joker));
    }

    #region EVENT TESTS

    [Test]
    public void TestEvents()
    {
        deck.OnDrawCard += HandleDrawCard;
        deck.OnShuffledCards += HandleShuffleCards;
        deck.BuildStandardDeck();

        //test draw event call
        try
        {
            eventCalled = false;
            expectedDeckSize = 51;
            deck.Draw();
            expectedDeckSize = 50;
            deck.Draw(30);
            Assert.IsTrue(eventCalled);
            deck.OnDrawCard -= HandleDrawCard;
        }
        catch
        {
            deck.OnDrawCard -= HandleDrawCard;
        }

        //try shuffle event call
        try
        {
            eventCalled = false;
            firstCard = deck[0];
            lastCard = deck[deck.DeckSize - 1];
            deck.ShuffleDeck();
            Assert.IsTrue(eventCalled);
            deck.OnShuffledCards -= HandleShuffleCards;
        }
        catch
        {
            deck.OnShuffledCards -= HandleShuffleCards;
        }
    }

    public void HandleDrawCard(ICard card)
    {
        eventCalled = true;
        Assert.AreEqual(expectedDeckSize, deck.DeckSize);
    }

    public void HandleShuffleCards(IEnumerable<ICard> cards)
    {
        eventCalled = true;
        Assert.IsFalse(deck[0] == firstCard && deck[deck.DeckSize - 1] == lastCard); //ensure that the cards have been shuffled
    }

    #endregion

    [Test]
    public void TestDrawing()
    {
        deck = new CardDeck("A,B,C,D");
        Assert.AreEqual("A", deck.Draw().Code);
        Assert.AreEqual("B", deck.Draw().Code);
        Assert.AreEqual("C", deck.Draw().Code);
        Assert.AreEqual("D", deck.Draw().Code);

        deck = new CardDeck("A,B,C,D");
        Assert.AreEqual("D", deck.Draw(3).Code);
        Assert.AreEqual("C", deck.Draw(2).Code);
        Assert.AreEqual("B", deck.Draw(1).Code);
        Assert.AreEqual("A", deck.Draw(0).Code);

        deck = new CardDeck(new Card("A"), new Card("B"));
        Assert.AreEqual(new Card("A"), deck.Draw());
    }

    [Test]
    public void TestDiscarding()
    {
        deck = new CardDeck("A,B,C");
        var card = new Card("D");
        deck.Discard(card);
        Assert.AreEqual(4, deck.DeckSize);
        Assert.AreEqual("D", deck[3].Code);
    }

    [Test]
    public void TestInserting()
    {
        var card = new Card("E");
        deck.Insert(card, 2);
        Assert.AreEqual("A,B,E,C,D", deck.ToString());

        Assert.Throws<IndexOutOfRangeException>(() => deck.Insert(card, -1));
        Assert.Throws<IndexOutOfRangeException>(() => deck.Insert(card, 5));
    }

    [Test]
    public void TestIndex()
    {
        Assert.DoesNotThrow(() =>
        {
            deck = new CardDeck("A,B,C,D");
            Assert.AreEqual("A", deck[0].Code);
            Assert.AreEqual("B", deck[1].Code);
            Assert.AreEqual("C", deck[2].Code);
            Assert.AreEqual("D", deck[3].Code);
        });

        Assert.Throws<IndexOutOfRangeException>(() => Debug.Log(deck[-1]));
        Assert.Throws<IndexOutOfRangeException>(() => Debug.Log(deck[5]));
    }

    [Test]
    public void TestEnumeration()
    {
        var expectedQueue = new Queue<string>();
        expectedQueue.Enqueue("A");
        expectedQueue.Enqueue("B");
        expectedQueue.Enqueue("C");
        expectedQueue.Enqueue("D");

        deck = new CardDeck("A,B,C,D");
        foreach (ICard card in deck)
            Assert.AreEqual(expectedQueue.Dequeue(), card.Code);

        deck.Init(null);
        Assert.DoesNotThrow(() =>
        {
            foreach (var _ in deck) { }
        });
    }

    [Test]
    public void TestShuffleCards()
    {
        deck.BuildStandardDeck();
        var initialString = deck.ToString();
        deck.ShuffleDeck();
        Assert.AreNotEqual(initialString, deck.ToString());
    }

    [Test]
    public void TestAddingCards()
    {
        IDeck deckA = new CardDeck("A,B,C");
        IDeck deckB = new CardDeck("D,E,F,G");

        deckB.AddCardsFromDeck(ref deckA, 2);
        Assert.AreEqual(1, deckA.DeckSize);
        Assert.AreEqual(6, deckB.DeckSize);

        Assert.AreEqual("C", deckA.ToString());
        Assert.AreEqual("D,E,F,G,A,B", deckB.ToString());
    }

    [Test]
    public void TestCombineDeck()
    {
        IDeck deckA = new CardDeck("A,B,C");
        IDeck deckB = new CardDeck("D,E,F,G");

        deckB.CombineDeck(deckA);
        Assert.AreEqual(3, deckA.DeckSize);
        Assert.AreEqual(7, deckB.DeckSize);
        Assert.AreEqual("D,E,F,G,A,B,C", deckB.ToString());
    }

    [Test]
    public void TestClear()
    {
        deck.BuildStandardDeck();
        Assert.IsFalse(deck.IsEmpty);
        deck.ClearDeck();
        Assert.IsTrue(deck.IsEmpty);
    }

    [Test]
    public void TestPredicates()
    {
        deck.BuildStandardDeck(true);

        //counting
        Assert.AreEqual(4, deck.Count(x => x.Code.Contains("X")));
        Assert.AreEqual(14, deck.Count(x => int.Parse("" + x.Code[0]) == (int)CardSuit.Diamonds));
        Assert.AreEqual(0, deck.Count(x => x.Code == "Blah"));

        //finding
        Assert.IsNotNull(deck.Find(x => x.Code == "0A"));
        Assert.IsNotNull(deck.Find(x => x.Code == "2X"));
        Assert.IsNotNull(deck.Find(x => x.Code == "3Q"));
        Assert.IsNotNull(deck.Find(x => x.Code == "0K"));
        Assert.IsNull(deck.Find(x => x.Code == "A"));

        //find all
        Assert.AreEqual(4, deck.FindAll(x => x.Code.Contains("9")).Count());
        Assert.AreEqual(1, deck.FindAll(x => x.Code.Contains("33")).Count());
    }

    [Test]
    public void TestHasCard()
    {
        deck.BuildStandardDeck();
        Assert.IsTrue(deck.HasCard("12"));
        Assert.IsFalse(deck.HasCard("11")); //ace is not 1
        Assert.IsFalse(deck.HasCard("AA"));

        Assert.IsTrue(deck.HasCard('2'));
        Assert.IsFalse(deck.HasCard('4'));

        Assert.IsTrue(deck.HasCard(CardSuit.Clubs, CardFace.Four));
        Assert.IsFalse(deck.HasCard(CardSuit.Clubs, CardFace.Joker));

        Assert.IsTrue(deck.HasCard(new Card(CardSuit.Spades, CardFace.Six)));
        Assert.IsFalse(deck.HasCard(new Card("A")));
    }

    [Test]
    public void TestIndicies()
    {
        deck = new CardDeck("A,B,C,C,D,27,27,A");

        Assert.AreEqual(0, deck.IndexOf("A"));
        Assert.AreEqual(5, deck.IndexOf(CardSuit.Spades, CardFace.Seven));
        Assert.AreEqual(2, deck.IndexOf(new Card("C")));

        Assert.AreEqual(7, deck.LastIndexOf(new Card("A")));
        Assert.AreEqual(3, deck.LastIndexOf("C"));
        Assert.AreEqual(6, deck.LastIndexOf(CardSuit.Spades, CardFace.Seven));
    }

    [Test]
    public void TestConversions()
    {
        deck = new CardDeck("A,B,C,D");

        //test lists
        var cardsList = deck.ToList();
        Assert.AreEqual("A", cardsList[0].Code);
        Assert.AreEqual("B", cardsList[1].Code);
        Assert.AreEqual("C", cardsList[2].Code);
        Assert.AreEqual("D", cardsList[3].Code);

        var testCardsList = deck.ToList<TestCard>();
        Assert.AreEqual("A", testCardsList[0].Code);
        Assert.AreEqual("B", testCardsList[1].Code);
        Assert.AreEqual("C", testCardsList[2].Code);
        Assert.AreEqual("D", testCardsList[3].Code);

        //test arrays
        var cardsArray = deck.ToArray();
        Assert.AreEqual("A", cardsArray[0].Code);
        Assert.AreEqual("B", cardsArray[1].Code);
        Assert.AreEqual("C", cardsArray[2].Code);
        Assert.AreEqual("D", cardsArray[3].Code);

        var testCardsArray = deck.ToArray<TestCard>();
        Assert.AreEqual("A", testCardsArray[0].Code);
        Assert.AreEqual("B", testCardsArray[1].Code);
        Assert.AreEqual("C", testCardsArray[2].Code);
        Assert.AreEqual("D", testCardsArray[3].Code);
    }

    [Test]
    public void TestToString()
    {
        deck.BuildStandardDeck();
        var deckString = deck.ToString();
        Assert.AreNotEqual(string.Empty, deckString);
        Assert.IsTrue(deckString.Contains(","));
        Assert.IsFalse(deckString.LastIndexOf(",") == deckString.Length - 1);
        deck.ClearDeck();
        Assert.AreEqual(string.Empty, deck.ToString());
    }

    [Test]
    public void TestEquality()
    {
        var deckA = new CardDeck("A,B,C,D");
        var deckB = new CardDeck("A,B,C,D");
        var deckC = new CardDeck();
        var deckD = new CardDeck();

        Assert.IsTrue(deckA.Equals(deckB));
        Assert.IsFalse(deckA.Equals(deckC));
        Assert.IsTrue(deckC.Equals(deckD));
    }
}

internal class TestCard : Card
{
    public TestCard() : base() { }
}