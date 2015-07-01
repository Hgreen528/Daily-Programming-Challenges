//Author: Hunter Green
//Created: 6/25/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Medium_Challenge_215
{
    class Program
    {
        static void Main(string[] args)
        {
            //Input the sorting network and comparators
            //Console.WriteLine("Enter the number of wires and number of comparators seperated by a space: ");
            string[] allInputs = File.ReadAllLines("input1.txt");
            string[] inputs = allInputs[0].Split(' ');
            int wires = Int32.Parse(inputs[0]);
            int comparatorCount = Int32.Parse(inputs[1]);

            Comparator[] comparators = new Comparator[comparatorCount];

            //Console.WriteLine("Enter the wires compared by the comparators seperated by a space: ");
            for (int i = 1; i < comparatorCount + 1; i++)
            {
                inputs = allInputs[i].Split(' ');

                comparators[i - 1].wire1 = Int32.Parse(inputs[0]);
                comparators[i - 1].wire2 = Int32.Parse(inputs[1]);
            }

            //Evaluate if all 2^n combinations of 0's and 1's sort correctly
            int[] combination = new int[wires];
            int[] wireData = new int[wires];
            int[] result = combination;
            bool validSortingNetwork = true;
            while (combination.Contains(0))
            {
                for (int i = 0; i < wires; i++)
                    wireData[i] = combination[i];

                //Check if the combination is sorted
                if (!isValidSortingNetwork(wireData, comparators))
                {
                    validSortingNetwork = false;
                    break;
                }

                //Generate a new combination
                combination = setNextCombination(combination);
            }

            if (validSortingNetwork)
                Console.WriteLine("Valid Network");
            else
                Console.WriteLine("Invalid Network");

            Console.ReadLine();
        }

        static int[] setNextCombination(int[] comparison)
        {
            for (int i = 0; i < comparison.Length; i++)
            {
                if (comparison[i] == 0)
                {
                    comparison[i] = 1;
                    break;
                }
                else
                {
                    comparison[i] = 0;
                }
            }

            return comparison;
        }

        static bool isValidSortingNetwork(int[] combination, Comparator[] comparators)
        {
            //Run the sorting network on the combination
            for (int i = 0; i < comparators.Length; i++)
            {
                if (combination[comparators[i].wire1] < combination[comparators[i].wire2])
                {
                    int temp = combination[comparators[i].wire1];
                    combination[comparators[i].wire1] = combination[comparators[i].wire2];
                    combination[comparators[i].wire2] = temp;
                }
            }

            for (int i = 0; i < combination.Length-1; i++)
            {
                if (combination[i] < combination[i + 1])
                    return false;
            }

            return true;
        }

        struct Comparator
        {
            public int wire1;
            public int wire2;
        }
    }
}
