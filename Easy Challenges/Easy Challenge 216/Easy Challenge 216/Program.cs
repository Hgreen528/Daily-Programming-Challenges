//Author: Hunter Green
//Created: 6/28/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy_Challenge_216
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many players (2-8)? ");
            int playerCount = Int32.Parse(Console.ReadLine());

            Deck deck = new Deck();

            Player player = new Player();
            player.AddCard(deck.draw());
            player.AddCard(deck.draw());

            Console.Write("Your hand: ");
            player.PrintHand();

            List<Player> CPUs = new List<Player>();

            for(int i = 1; i < playerCount; i++)
            {
                CPUs.Add(new Player());
                CPUs[i - 1].AddCard(deck.draw());
                CPUs[i - 1].AddCard(deck.draw());
                Console.Write("CPU " + i + " Hand: ");
                CPUs[i - 1].PrintHand();
            }

            Console.WriteLine();

            Console.Write("Flop: ");
            deck.draw().Print();
            Console.Write(", ");
            deck.draw().Print();
            Console.Write(", ");
            deck.draw().Print();
            Console.Write("\n");

            Console.Write("Turn: ");
            deck.draw().Print();
            Console.Write("\n");

            Console.Write("River: ");
            deck.draw().Print();
            Console.Write("\n");

            Console.ReadLine();
        }
    }

    class Player
    {
        List<Card> hand;

        public Player()
        {
            hand = new List<Card>();
        }

        public void AddCard(Card c)
        {
            hand.Add(c);
        }

        public void PrintHand()
        {
            hand[0].Print();
            Console.Write(", ");
            hand[1].Print();
            Console.Write("\n");
        }
    }

    class Deck
    {
        List<Card> deck;
        const int DECK_SIZE = 52;

        public Deck()
        {
            deck = new List<Card>();

            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 13; j++)
                {
                    deck.Add(new Card(i, j));
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            Random r = new Random();
            for(int i = 0; i < DECK_SIZE; i++)
            {
                int swapCard = r.Next(i, DECK_SIZE);
                Card temp = deck[i];
                deck[i] = deck[swapCard];
                deck[swapCard] = temp;
            }
        }

        public Card draw()
        {
            Card c = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);
            return c;
        }
    }

    class Card
    {
        public enum Suit
        {
            Spades,
            Clubs,
            Hearts,
            Diamonds
        }

        public enum Number
        {
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        }

        Suit cardSuit;
        Number cardNumber;

        public Card(int s, int n)
        {
            switch(s)
            {
                case 0:
                    cardSuit = Suit.Spades;
                    break;
                case 1:
                    cardSuit = Suit.Clubs;
                    break;
                case 2:
                    cardSuit = Suit.Hearts;
                    break;
                case 3:
                    cardSuit = Suit.Diamonds;
                    break;
            }

            switch(n)
            {
                case 0:
                    cardNumber = Number.Ace;
                    break;
                case 1:
                    cardNumber = Number.Two;
                    break;
                case 2:
                    cardNumber = Number.Three;
                    break;
                case 3:
                    cardNumber = Number.Four;
                    break;
                case 4:
                    cardNumber = Number.Five;
                    break;
                case 5:
                    cardNumber = Number.Six;
                    break;
                case 6:
                    cardNumber = Number.Seven;
                    break;
                case 7:
                    cardNumber = Number.Eight;
                    break;
                case 8:
                    cardNumber = Number.Nine;
                    break;
                case 9:
                    cardNumber = Number.Ten;
                    break;
                case 10:
                    cardNumber = Number.Jack;
                    break;
                case 11:
                    cardNumber = Number.Queen;
                    break;
                case 12:
                    cardNumber = Number.King;
                    break;

            }
        }

        public Card(Suit s, Number n)
        {
            cardSuit = s;
            cardNumber = n;
        }

        Suit CardSuit
        {
            get { return cardSuit; }
        }

        Number CardNumber
        {
            get { return cardNumber; }
        }

        public void Print()
        {
            Console.Write(CardNumber + " of " + cardSuit);
        }
    }
}
