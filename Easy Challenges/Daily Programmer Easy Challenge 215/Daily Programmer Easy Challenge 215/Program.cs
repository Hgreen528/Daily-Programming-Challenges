//Author: Hunter Green
//Created: 6/23/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daily_Programmer_Easy_Challenge_215
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the sad cycle finder!");
            Console.WriteLine("Enter the sad cycle base:");
            int sadCycleBase = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Enter the starting number: ");
            long currentNumber = Int32.Parse(Console.ReadLine());

            findSadCycle(sadCycleBase, currentNumber);

            Console.ReadLine();
        }

        static void findSadCycle(int sadCycleBase, long currentNumber)
        {
            List<long> numberChain = new List<long>();

            //Evaluate numbers until a cycle occurs
            do
            {
                //Add the current number to the chain
                numberChain.Add(currentNumber);

                //Calculate the next number
                long newNumber = 0;
                for (long place = 1; place <= currentNumber; place *= 10)
                {
                    newNumber += Power(getDigit(currentNumber, place), sadCycleBase);
                }

                currentNumber = newNumber;

                //Check if a cycle has occured
            } while (!numberChain.Contains(currentNumber));

            //Find the index of the current number/beginning of the cycle
            int index = 0;
            for (int i = 0; i < numberChain.Count; i++)
            {
                if (numberChain[i] == currentNumber)
                {
                    index = i;
                    break;
                }
            }

            //Print the cycle
            Console.Write("The cycle is: ");

            for (; index < numberChain.Count - 1; index++)
            {
                Console.Write(numberChain[index] + ", ");
            }

            Console.Write(numberChain[index]);
        }

        static int getDigit(long number, long place)
        {
            return (int)((number / place)%10);
        }

        static long Power(long number, int exponent)
        {
            long pow = number;
            for (int i = 1; i < exponent; i++)
            {
                pow *= number;
                
            }

            return pow;
        }
    }
}
