//Author: Hunter Green
//Created: 6/29/15
//Extension of Easy Challenge 216, some bugs still exist
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medium_Challenge_216
{
    class Program
    {
        public static List<Card> flop;
        public static Card Turn;
        public static Card River;

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

            for (int i = 1; i < playerCount; i++)
            {
                CPUs.Add(new Player());
                CPUs[i - 1].AddCard(deck.draw());
                CPUs[i - 1].AddCard(deck.draw());
                Console.Write("CPU " + i + " Hand: ");
                CPUs[i - 1].PrintHand();
            }

            Console.WriteLine();

            flop = new List<Card>();

            Console.Write("Flop: ");
            flop.Add(deck.draw());
            flop[0].Print();
            Console.Write(", ");
            flop.Add(deck.draw());
            flop[1].Print();
            Console.Write(", ");
            flop.Add(deck.draw());
            flop[2].Print();
            Console.Write("\n");

            List<Player> remainingPlayers = new List<Player>();
            List<Player> foldedPlayers = new List<Player>();
            remainingPlayers.Add(player);
            for (int i = 1; i < playerCount; i++)
            {
                //Determine who folds
                if(CPUs[i-1].IsFolding())
                {
                    Console.WriteLine("CPU " + i + " Folds!");
                    foldedPlayers.Add(CPUs[i - 1]);
                }
                else
                {
                    remainingPlayers.Add(CPUs[i - 1]);
                }
            }
            
            Console.Write("Turn: ");
            Turn = deck.draw();
            Turn.Print();
            Console.Write("\n");

            Console.Write("River: ");
            River = deck.draw();
            River.Print();
            Console.Write("\n");

            //Determine final hands and print who won
            int winner = 0;
            player.DetermineFinalHandValueAndType();
            long winningHandValue = player.handValue;
            for (int i = 1; i < remainingPlayers.Count; i++)
            {
                remainingPlayers[i].DetermineFinalHandValueAndType();
                if(remainingPlayers[i].handValue > winningHandValue)
                {
                    winningHandValue = remainingPlayers[i].handValue;
                    for(int j = 0; j < playerCount-1; j++)
                    {
                        if (CPUs[j].handValue == winningHandValue)
                            winner = i;
                    }
                }
            }
            Console.WriteLine();
            if (winner == 0)
                Console.WriteLine("Winner: Player. " + player.handType);
            else
                Console.WriteLine("Winner: CPU " + winner + ". " + remainingPlayers[winner].handType);

            //Determine who had the best hand and print
            for (int i = 1; i < foldedPlayers.Count; i++)
            {
                foldedPlayers[i].DetermineFinalHandValueAndType();
                if (foldedPlayers[i].handValue > winningHandValue)
                {
                    winningHandValue = foldedPlayers[i].handValue;
                    for (int j = 0; j < playerCount - 1; j++)
                    {
                        if (CPUs[j].handValue == winningHandValue)
                            winner = i;
                    }
                }
            }

            if (winner == 0)
                Console.WriteLine("Best Hand: Player");
            else
                Console.WriteLine("Best Hand: CPU " + winner + ".");

            Console.ReadLine();
        }

        //Seperates hand categories by orders of magnitude; high card is 100 + card value, one pair is 1000 + pair value, etc.
        public static long EvaluateHand(List<Card> hand)
        {
            if (hand.Count != 5)
                return 0;

            long handValue = 0;

            //Determine Category
            //Check for a straight flush
            bool flush = true;
            for (int i = 0; i < hand.Count - 1; i++)
            {
                if (hand[i].CardSuit != hand[i + 1].CardSuit)
                {
                    flush = false;
                }
            }

            bool straight = true;
            hand.Sort(Card.SortByCardNumer);
            for (int i = 0; i < hand.Count - 1; i++)
            {
                if ((int)hand[i].CardNumber + 1 != (int)hand[i + 1].CardNumber)
                {
                    straight = false;
                }
            }

            if (straight && flush)
            {
                handValue = 10000000000;
                handValue += (int)hand[hand.Count - 1].CardNumber;
                return handValue;
            }

            //Check for four of a kind
            Dictionary<Card.Number, int> numberCounts = new Dictionary<Card.Number, int>();
            numberCounts.Add(Card.Number.Ace, 0);
            numberCounts.Add(Card.Number.Two, 0);
            numberCounts.Add(Card.Number.Three, 0);
            numberCounts.Add(Card.Number.Four, 0);
            numberCounts.Add(Card.Number.Five, 0);
            numberCounts.Add(Card.Number.Six, 0);
            numberCounts.Add(Card.Number.Seven, 0);
            numberCounts.Add(Card.Number.Eight, 0);
            numberCounts.Add(Card.Number.Nine, 0);
            numberCounts.Add(Card.Number.Ten, 0);
            numberCounts.Add(Card.Number.Jack, 0);
            numberCounts.Add(Card.Number.King, 0);
            numberCounts.Add(Card.Number.Queen, 0);

            foreach (Card c in hand)
            {
                numberCounts[c.CardNumber]++;
            }

            bool fourOfAKind = false;
            if (numberCounts[Card.Number.Ace] == 4
                || numberCounts[Card.Number.Two] == 4
                || numberCounts[Card.Number.Three] == 4
                || numberCounts[Card.Number.Four] == 4
                || numberCounts[Card.Number.Five] == 4
                || numberCounts[Card.Number.Six] == 4
                || numberCounts[Card.Number.Seven] == 4
                || numberCounts[Card.Number.Eight] == 4
                || numberCounts[Card.Number.Nine] == 4
                || numberCounts[Card.Number.Ten] == 4
                || numberCounts[Card.Number.Jack] == 4
                || numberCounts[Card.Number.Queen] == 4
                || numberCounts[Card.Number.King] == 4)
            {
                fourOfAKind = true;
            }
            
            if(fourOfAKind)
            {
                handValue = 1000000000;

                if ((int)hand[0].CardNumber > (int)hand[1].CardNumber)
                    handValue += (int)hand[0].CardNumber;
                else
                    handValue += (int)hand[1].CardNumber;

                return handValue;
            }

            //Check for a full house
            bool threeOfAKind = false;
            Card.Number threeOfAKindNumber = Card.Number.Two;
            if(numberCounts[Card.Number.Two] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Two;
            }
            if (numberCounts[Card.Number.Three] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Three;
            }
            if (numberCounts[Card.Number.Four] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Four;
            }
            if (numberCounts[Card.Number.Five] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Five;
            }
            if (numberCounts[Card.Number.Six] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Six;
            }
            if (numberCounts[Card.Number.Seven] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Seven;
            }
            if (numberCounts[Card.Number.Eight] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Eight;
            }
            if (numberCounts[Card.Number.Nine] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Nine;
            }
            if (numberCounts[Card.Number.Ten] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Ten;
            }
            if (numberCounts[Card.Number.Jack] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Jack;
            }
            if (numberCounts[Card.Number.Queen] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Queen;
            }
            if (numberCounts[Card.Number.King] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.King;
            }
            if (numberCounts[Card.Number.Ace] == 3)
            {
                threeOfAKind = true;
                threeOfAKindNumber = Card.Number.Ace;
            }

            bool twoOfAKind = false;
            Card.Number twoOfAKindNumber = Card.Number.Two;

            if (numberCounts[Card.Number.Two] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Two;
            }
            if (numberCounts[Card.Number.Three] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Three;
            }
            if (numberCounts[Card.Number.Four] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Four;
            }
            if (numberCounts[Card.Number.Five] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Five;
            }
            if (numberCounts[Card.Number.Six] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Six;
            }
            if (numberCounts[Card.Number.Seven] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Seven;
            }
            if (numberCounts[Card.Number.Eight] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Eight;
            }
            if (numberCounts[Card.Number.Nine] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Nine;
            }
            if (numberCounts[Card.Number.Ten] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Ten;
            }
            if (numberCounts[Card.Number.Jack] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Jack;
            }
            if (numberCounts[Card.Number.Queen] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Queen;
            }
            if (numberCounts[Card.Number.King] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.King;
            }
            if (numberCounts[Card.Number.Ace] == 2)
            {
                twoOfAKind = true;
                twoOfAKindNumber = Card.Number.Ace;
            }

            if (twoOfAKind && threeOfAKind)
            {
                handValue = 100000000;
                handValue += (int)threeOfAKindNumber;
                return handValue;
            }

            //Check for a flush
            if (flush)
            {
                handValue = 10000000;
                handValue += (int)hand[hand.Count - 1].CardNumber;
                return handValue;
            }

            //Check for a straight
            if(straight)
            {
                handValue = 1000000;
                handValue += (int)hand[hand.Count - 1].CardNumber;
                return handValue;
            }

            //Check for three of a kind
            if (threeOfAKind)
            {
                handValue = 100000;
                handValue += (int)threeOfAKindNumber;
                return handValue;
            }

            //Check for two pair
            int pairs = 0;
            if (numberCounts[Card.Number.Ace] == 2)
                pairs++;
            if (numberCounts[Card.Number.Two] == 2)
                pairs++;
            if (numberCounts[Card.Number.Three] == 2)
                pairs++;
            if (numberCounts[Card.Number.Four] == 2)
                pairs++;
            if (numberCounts[Card.Number.Five] == 2)
                pairs++;
            if (numberCounts[Card.Number.Six] == 2)
                pairs++;
            if (numberCounts[Card.Number.Seven] == 2)
                pairs++;
            if (numberCounts[Card.Number.Eight] == 2)
                pairs++;
            if (numberCounts[Card.Number.Nine] == 2)
                pairs++;
            if (numberCounts[Card.Number.Ten] == 2)
                pairs++;
            if (numberCounts[Card.Number.Jack] == 2)
                pairs++;
            if (numberCounts[Card.Number.Queen] == 2)
                pairs++;
            if (numberCounts[Card.Number.King] == 2)
                pairs++;

            if(pairs == 2)
            {
                handValue = 10000;
                handValue += (int)twoOfAKindNumber;
                return handValue;
            }

            //Check for one pair
            if (twoOfAKind)
            {
                handValue = 1000;
                handValue += (int)twoOfAKindNumber;
                return handValue;
            }

            //Evaluate high card
            return 100 + (int)hand[hand.Count-1].CardNumber;
        }

        static int max(int a, int b)
        {
            if (a > b)
                return a;
            else
                return b;
        }
    }

    class Player
    {
        List<Card> hand;

        public bool folded;
        public long handValue;
        public enum HandType
        {
            High_Card,
            One_Pair,
            Two_Pair,
            Three_of_a_Kind,
            Straight,
            Flush,
            Full_House,
            Four_of_a_Kind,
            Straight_Flush
        }
        public HandType handType;

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

        public bool IsFolding()
        {
            List<Card> fullHand = new List<Card>();
            fullHand.AddRange(hand);
            fullHand.AddRange(Program.flop);

            handValue = Program.EvaluateHand(fullHand);

            if (handValue < 100)
            {
                folded = true;
                return true;
            }

            folded = false;
            return false;
        }

        public void DetermineFinalHandValueAndType()
        {
            //Iterate through each combination and find the maximum value hand
            //First choose both cards in hand, then one card in hand, and finally no cards in hand
            //Then set the hand value and type
            List<Card> allCards = new List<Card>();
            allCards.AddRange(hand);
            allCards.AddRange(Program.flop);
            allCards.Add(Program.River);
            allCards.Add(Program.Turn);
            List<Card> fullHand = new List<Card>();
            bool[] cardsTaken = new bool[7] { true, true, true, true, true, false, false};

            for (int i = 0; i < 7; i++)
            {
                if (cardsTaken[i])
                    fullHand.Add(allCards[i]);
            }

            handValue = Program.EvaluateHand(fullHand);

            if (handValue >= 10000000000)
                handType = HandType.Straight_Flush;
            else if (handValue >= 1000000000)
                handType = HandType.Four_of_a_Kind;
            else if (handValue >= 100000000)
                handType = HandType.Full_House;
            else if (handValue >= 10000000)
                handType = HandType.Flush;
            else if (handValue >= 1000000)
                handType = HandType.Straight;
            else if (handValue >= 100000)
                handType = HandType.Three_of_a_Kind;
            else if (handValue >= 10000)
                handType = HandType.Two_Pair;
            else if (handValue >= 1000)
                handType = HandType.One_Pair;
            else
                handType = HandType.High_Card;

            for (int i = 0; i < 20; i++)
            {
                cardsTaken = incrementChoice(cardsTaken, i);

                for (int j = 0; j < 7; j++)
                {
                    if (cardsTaken[j])
                        fullHand.Add(allCards[j]);
                }

                long tempHandValue = Program.EvaluateHand(fullHand);
                if(tempHandValue > handValue)
                {
                    handValue = tempHandValue;

                    if (handValue >= 10000000000)
                        handType = HandType.Straight_Flush;
                    else if (handValue >= 1000000000)
                        handType = HandType.Four_of_a_Kind;
                    else if (handValue >= 100000000)
                        handType = HandType.Full_House;
                    else if (handValue >= 10000000)
                        handType = HandType.Flush;
                    else if (handValue >= 1000000)
                        handType = HandType.Straight;
                    else if (handValue >= 100000)
                        handType = HandType.Three_of_a_Kind;
                    else if (handValue >= 10000)
                        handType = HandType.Two_Pair;
                    else if (handValue >= 1000)
                        handType = HandType.One_Pair;
                    else
                        handType = HandType.High_Card;
                }
            }
        }

        bool[] incrementChoice(bool[] cardsTaken, int increment)
        {
            switch(increment)
            {
                case 0:
                    cardsTaken = new bool[7] { true, true, true, true, false, true, false };
                    break;
                case 1:
                    cardsTaken = new bool[7] { true, true, true, true, false, false, true };
                    break;
                case 2:
                    cardsTaken = new bool[7] { true, true, true, false, true, true, false };
                    break;
                case 3:
                    cardsTaken = new bool[7] { true, true, true, false, true, false, true };
                    break;
                case 4:
                    cardsTaken = new bool[7] { true, true, true, false, false, true, true };
                    break;
                case 5:
                    cardsTaken = new bool[7] { true, true, false, true, true, true, false };
                    break;
                case 6:
                    cardsTaken = new bool[7] { true, true, false, true, true, false, true };
                    break;
                case 7:
                    cardsTaken = new bool[7] { true, true, false, true, false, true, true };
                    break;
                case 8:
                    cardsTaken = new bool[7] { true, true, false, false, true, true, true };
                    break;
                case 9:
                    cardsTaken = new bool[7] { true, false, true, true, true, true, false };
                    break;
                case 10:
                    cardsTaken = new bool[7] { true, false, true, true, true, false, true };
                    break;
                case 11:
                    cardsTaken = new bool[7] { true, false, true, true, false, true, true };
                    break;
                case 12:
                    cardsTaken = new bool[7] { true, false, true, false, true, true, true };
                    break;
                case 13:
                    cardsTaken = new bool[7] { true, false, false, true, true, true, true };
                    break;
                case 14:
                    cardsTaken = new bool[7] { false, true, true, true, true, true, false };
                    break;
                case 15:
                    cardsTaken = new bool[7] { false, true, true, true, true, false, true };
                    break;
                case 16:
                    cardsTaken = new bool[7] { false, true, true, true, false, true, true };
                    break;
                case 17:
                    cardsTaken = new bool[7] { false, true, true, false, true, true, true };
                    break;
                case 18:
                    cardsTaken = new bool[7] { false, true, false, true, true, true, true };
                    break;
                case 19:
                    cardsTaken = new bool[7] { false, false, true, true, true, true, true };
                    break;
            }

            return cardsTaken;
        }
    }

    class Deck
    {
        List<Card> deck;
        const int DECK_SIZE = 52;

        public Deck()
        {
            deck = new List<Card>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    deck.Add(new Card(i, j));
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            Random r = new Random();
            for (int i = 0; i < DECK_SIZE; i++)
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
            King,
            Ace
        }

        Suit cardSuit;
        Number cardNumber;

        public Card(int s, int n)
        {
            switch (s)
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

            switch (n)
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

        public Suit CardSuit
        {
            get { return cardSuit; }
        }

        public Number CardNumber
        {
            get { return cardNumber; }
        }

        public void Print()
        {
            Console.Write(CardNumber + " of " + cardSuit);
        }

        public static int SortByCardNumer(Card a, Card b)
        {
            if ((int)a.cardNumber > (int)b.cardNumber)
                return 1;
            else
                return -1;
        }
    }
}
