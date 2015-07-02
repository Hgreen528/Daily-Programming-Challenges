//Author: Hunter Green
//Created: 6/30/15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Word Snake: Takes in a sequence of words in all caps where the first letter of the next word is the same
//as the last letter of the previous. Prints a word snake where each word is rotated ninety degrees from the previous.
namespace Easy_Challenge_221
{
    class Program
    {
        static Random random;

        static void Main(string[] args)
        {
            random = new Random();

            Console.WriteLine("Enter the words to snake in all caps seperated by spaces." +
                "\nEach subsequent word needs to start with the same letter as the previous word.");
            string[] words = Console.ReadLine().Split(' ');

            //Create a map to set to the words in, and determine the necessary width and height of the map
            char[,] letterMap;
            int totalWordLength = 0;
            foreach (string word in words)
            {
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

            if(random.Next(0, 2) == 0)
            {
                //Start with right
                MakeWordSnake(words, 0, 1, 0, 0, 0, letterMap);
            }
            else
            {
                //Start with down
                MakeWordSnake(words, 0, 0, 1, 0, 0, letterMap);
            }
            Console.ReadLine();
        }
        
        

        //recurse down through options, and return a boolean false if the option does not work. Assumes the map is large enough to hold all the words in at least one form
        static bool MakeWordSnake(string[] words, int currentWord, int stepX, int stepY, int currentX, int currentY, char[,] map)
        {
            //loop through words[current word] adding each char to map along the specified direction
            //if it runs into another word or runs out of room, returns false
            map[currentX, currentY] = words[currentWord][0];

            //See if the word fits
            if(currentX + stepX * words[currentWord].Length < map.GetLength(0) && currentX + stepX * words[currentWord].Length >= 0
                && currentY + stepY * words[currentWord].Length < map.GetLength(1) && currentY + stepY * words[currentWord].Length >= 0)
            {
                //Ensure the word will not intersect other words
                for(int i = 1; i < words[currentWord].Length; i++)
                {
                    if (map[currentX + stepX*i, currentY + stepY*i] != ' ')
                        return false;
                }
                
                //Add the word in
                for (int i = 1; i < words[currentWord].Length; i++)
                {
                    currentX += stepX;
                    currentY += stepY;
                    map[currentX, currentY] = words[currentWord][i];
                }
            }
            else
            {
                //The word does not fit the map
                return false;
            }

            //if there are more words to add
            if (currentWord < words.Length - 1)
            {
                //after looping through, randomly shuffle a set of directions and try each in the new random order. If one succeeds (all the way down) return true

                //Change the direction by ninety degrees (swap the steps)
                int temp = stepX;
                stepX = stepY;
                stepY = temp;

                if(random.Next(0, 2) == 0)
                {
                    //Reverse the steps to go in the opposite direction
                    stepX = -stepX;
                    stepY = -stepY;
                }

                if(MakeWordSnake(words, currentWord + 1, stepX, stepY, currentX, currentY, map))
                {
                    return true;
                }
                else
                {
                    return MakeWordSnake(words, currentWord + 1, -stepX, -stepY, currentX, currentY, map);
                }
            }
            else
            {
                //print the word snake and return true
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        Console.Write(map[i, j]);
                    }
                    Console.WriteLine();
                }

                return true;
            }
        }
    }
}
