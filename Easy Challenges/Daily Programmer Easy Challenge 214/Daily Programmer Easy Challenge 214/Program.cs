//Author: Hunter Green
//Created: 6/23/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daily_Programmer_Easy_Challenge_214
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("You have entered the domain of population standard deviation calculation. Proceed at your own risk.");
            Console.WriteLine("Enter all data points as integers seperated by spaces");
            string inputData = Console.ReadLine();
            
            //Parse the input into integers
            List<int> data = new List<int>();
            for (int i = 0; i < inputData.Length; i++)
            {
                if (inputData[i] == ' ')
                {
                    data.Add(Int32.Parse(inputData.Substring(0, i)));
                    inputData = inputData.Substring(i + 1);
                    i = 0;
                }
            }
            data.Add(Int32.Parse(inputData));

            double stdDev = standardDeviation(data);
            
            //Round standard deviation to four decimals
            stdDev = (double)((int)(stdDev * 10000))/10000f;
            
            Console.WriteLine(stdDev);
            Console.ReadLine();
        }

        static double standardDeviation(List<int> population)
        {
            double stdDev = 0;
            long sumOfSquares = 0;
            double squareOfSums = 0;

            for (int i = 0; i < population.Count; i++)
            {
                sumOfSquares += population[i] * population[i];
                squareOfSums += population[i];
            }

            squareOfSums *= squareOfSums;

            Console.WriteLine("Square of sums = " + squareOfSums + ", Sum of Squares = " + sumOfSquares);

            double variance = (sumOfSquares - squareOfSums/population.Count)/population.Count;

            stdDev = Math.Sqrt(variance);

            return stdDev;
        }
    }
}
