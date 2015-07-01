//Author: Hunter Green
//Created: 6/30/15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Word Snake: Takes in a sequence of words in all caps where the first letter of the next word is the same
//as the last letter of the previous. Prints a word snake where each word is rotated ninety degrees from the previous.
//Can currently move in all four directions, however can run into situations where it cannot place more words
namespace Easy_Challenge_221
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the words to snake in all caps seperated by spaces." +
                "\nEach subsequent word needs to start with the same letter as the previous word.");
            string[] words = Console.ReadLine().Split(' ');

            //Create a map to set to the words in, and determine the necessary width and height of the map
            char[,] letterMap;
            int maxWordLength = 0;
            int minWordLength = 100;
            int totalWordLength = 0;
            foreach (string word in words)
            {
                if (word.Length > maxWordLength)
                    maxWordLength = word.Length;

                if (word.Length < minWordLength)
                    minWordLength = word.Length;

                totalWordLength += word.Length;
            }
            letterMap = new char[totalWordLength, totalWordLength];

            for (int i = 0; i < totalWordLength; i++)
            {
                for (int j = 0; j < totalWordLength; j++)
                {
                    letterMap[i, j] = ' ';
                }
            }

            //Insert the words into the map, starting at the top-left corner
            List<Direction> validDirections = new List<Direction>();
            validDirections.Add(Direction.Down);
            validDirections.Add(Direction.Right);
            Random rand = new Random();
            int[] currentPosition = new int[2] { 0, 0 };
            for (int i = 0; i < words.Length; i++)
            {
                //Determine the direction to move along and ensure the direction can be traversed
                int direction = rand.Next(validDirections.Count);

                try
                {
                    Direction d = validDirections[direction];
                }
                catch(Exception e)
                {
                    break;
                }

                switch (validDirections[direction])
                {
                    case Direction.Up:
                        for (int j = 0; j < words[i].Length; j++)
                        {
                            letterMap[currentPosition[0], currentPosition[1]] = words[i][j];
                            currentPosition[1]--;
                        }
                        currentPosition[1]++;
                        break;

                    case Direction.Down:
                        for (int j = 0; j < words[i].Length; j++)
                        {
                            letterMap[currentPosition[0], currentPosition[1]] = words[i][j];
                            currentPosition[1]++;
                        }
                        currentPosition[1]--;
                        break;

                    case Direction.Right:
                        for (int j = 0; j < words[i].Length; j++)
                        {
                            letterMap[currentPosition[0], currentPosition[1]] = words[i][j];
                            currentPosition[0]++;
                        }
                        currentPosition[0]--;
                        break;

                    case Direction.Left:
                        for (int j = 0; j < words[i].Length; j++)
                        {
                            letterMap[currentPosition[0], currentPosition[1]] = words[i][j];
                            currentPosition[0]--;
                        }
                        currentPosition[0]++;
                        break;
                }

                //Determine the valid directions for the next move
                Direction previousDirection = validDirections[direction];

                validDirections.Clear();

                if (previousDirection == Direction.Up || previousDirection == Direction.Down)
                {
                    validDirections.Add(Direction.Left);
                    validDirections.Add(Direction.Right);
                }
                else
                {
                    validDirections.Add(Direction.Up);
                    validDirections.Add(Direction.Down);
                }
                
                //Ensure the next word can fit in one of the valid directions
                //Also ensure the word after can fit
                if(i+1 < words.Length)
                {
                    int nextWordLength = words[i + 1].Length;

                    if(validDirections.Contains(Direction.Up) || validDirections.Contains(Direction.Down))
                    {
                        //Check up
                        validDirections.Add(Direction.Up);
                        for (int j = 1; j < nextWordLength; j++)
                        {
                            if (currentPosition[1] - j < 0 || letterMap[currentPosition[0], currentPosition[1] - j] != ' ')
                                validDirections.Remove(Direction.Up);
                        }

                        //Check down
                        validDirections.Add(Direction.Down);
                        for (int j = 1; j < nextWordLength; j++)
                        {
                            if (currentPosition[1] + j >= totalWordLength || letterMap[currentPosition[0], currentPosition[1] + j] != ' ')
                                validDirections.Remove(Direction.Down);
                        }
                    }
                    else
                    {
                        //Check right
                        validDirections.Add(Direction.Right);
                        for (int j = 1; j < nextWordLength; j++)
                        {
                            if (currentPosition[0] + j >= totalWordLength || letterMap[currentPosition[0] + j, currentPosition[1]] != ' ')
                                validDirections.Remove(Direction.Right);
                        }

                        //Check left
                        validDirections.Add(Direction.Left);
                        for (int j = 1; j < nextWordLength; j++)
                        {
                            if (currentPosition[0] - j < 0 || letterMap[currentPosition[0] - j, currentPosition[1]] != ' ')
                                validDirections.Remove(Direction.Left);
                        }
                    }
                }
            }


            for(int i = 0; i < totalWordLength; i++)
            {
                for(int j = 0; j < totalWordLength; j++)
                {
                    Console.Write(letterMap[i, j]);
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

    }
}
