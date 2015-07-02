//Author: Hunter Green
//Created: 7/2/15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//Takes in a string array containing a word snake (see easy challenge 221). 
//Unravels the word snake and prints the words which make up the word snake.
//Assumes at least one word is in the word snake
namespace Medium_Challenge_221
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("challenge input2.txt");

            foreach (string s in input)
                Console.WriteLine(s);

            //Move it to a character map
            char[,] letterMap;

            int maxLength = 0;
            foreach(string s in input)
            {
                if (s.Length > maxLength)
                    maxLength = s.Length;
            }

            letterMap = new char[maxLength, input.Length];
            for(int i = 0; i < maxLength; i++)
            {
                for(int j = 0; j < input.Length; j++)
                {
                    letterMap[i, j] = ' ';
                }
            }

            for(int y = 0; y < input.Length; y++)
            {
                for(int x = 0; x < input[y].Length; x++)
                {
                    letterMap[x, y] = input[y][x];
                }
            }

            string words = "";

            int currentX = 0;
            int currentY = 0;
            int stepX = 0;
            int stepY = 0;

            //Determine the starting direction
            if(letterMap[1, 0] != ' ') { //The start direction is to the right
                stepX = 1;
                stepY = 0;
            }
            else { //The start direction is down
                stepX = 0;
                stepY = 1;
            }

            bool moreWords = true;

            do {
                //Loop through the word
                while (InBounds(currentX, currentY, letterMap.GetLength(0), letterMap.GetLength(1))
                    && letterMap[currentX, currentY] != ' ') {
                    words += letterMap[currentX, currentY];
                    currentX += stepX;
                    currentY += stepY;
                }

                words += " "; //Keep a space between each word so the output is readable

                //Return to the last letter of the word added
                currentX -= stepX;
                currentY -= stepY;

                //Swap the steps for the ninety degree angle change
                int tempStep = stepX;
                stepX = stepY;
                stepY = tempStep;

                if (InBounds(currentX + stepX, currentY + stepY, letterMap.GetLength(0), letterMap.GetLength(1))
                    && letterMap[currentX + stepX, currentY + stepY] != ' ') {
                    //No change, the direction is correct
                }
                else if (InBounds(currentX - stepX, currentY - stepY, letterMap.GetLength(0), letterMap.GetLength(1))
                    && letterMap[currentX - stepX, currentY - stepY] != ' ') {
                    //The direction is opposite of the current steps, reverse them
                    stepX = -stepX;
                    stepY = -stepY;
                }
                else
                {
                    //If there is no next word, exit the loop
                    moreWords = false;
                }
            } while (moreWords);

            Console.WriteLine(words);

            Console.ReadLine();
        }

        static bool InBounds(int posX, int posY, int maxX, int maxY)
        {
            return (posX >= 0 && posX < maxX && posY >= 0 && posY < maxY);
        }
    }
}
