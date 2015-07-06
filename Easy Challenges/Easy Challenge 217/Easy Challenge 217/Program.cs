//Author: Hunter Green
//Created: 7/5/15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//The Lumberjack Pile Problem
//Takes in an nxn array of log piles and the number of logs to add
//Adds the logs to piles with the least logs, then outputs the new pile array
namespace Easy_Challenge_217
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input4.txt");

            int pileSize = Int32.Parse(input[0]);
            int[,] piles = new int[pileSize, pileSize];

            int logsRemaining = Int32.Parse(input[1]);

            IntPair[] pileIndexAndCount = new IntPair[pileSize * pileSize];

            for (int i = 0; i < pileSize; i++)
            {
                string[] line = input[i + 2].Split(' ');
                for (int j = 0; j < pileSize; j++)
                {
                    int count = Int32.Parse(line[j]);
                    piles[i, j] = count;
                    pileIndexAndCount[pileSize * i + j].index = 7 * pileSize + j;
                    pileIndexAndCount[pileSize * i + j].count = count;
                }
            }

            Array.Sort(pileIndexAndCount, IntPair.SortByCount);
            
            while(logsRemaining > 0)
            {
                //Add the log to the pile with the lowest count
                pileIndexAndCount[0].count++;

                //Float up the now larger pile to its appropriate place in the list
                for(int i = 0; i < pileIndexAndCount.Length-1; i++)
                {
                    if(pileIndexAndCount[i].count > pileIndexAndCount[i+1].count)
                    {
                        IntPair temp = pileIndexAndCount[i];
                        pileIndexAndCount[i] = pileIndexAndCount[i + 1];
                        pileIndexAndCount[i + 1] = temp;
                    }
                }

                logsRemaining--;
            }

            Array.Sort(pileIndexAndCount, IntPair.SortByIndex);

            for (int i = 0; i < pileIndexAndCount.Length; i++)
            {
                piles[i / pileSize, i % pileSize] = pileIndexAndCount[i].count;
            }

            for (int i = 0; i < pileSize; i++)
            {
                for(int j = 0; j < pileSize; j++)
                {
                    Console.Write(piles[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        struct IntPair
        {
            public int index;
            public int count;

            public static int SortByCount(IntPair a, IntPair b)
            {
                if (a.count > b.count)
                    return 1;
                else if (a.count < b.count)
                    return -1;
                else
                    return 0;
            }

            public static int SortByIndex(IntPair a, IntPair b)
            {
                if (a.index > b.index)
                    return 1;
                else if (a.index < b.index)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
