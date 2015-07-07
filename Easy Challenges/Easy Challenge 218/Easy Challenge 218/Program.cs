//Author: Hunter Green
//Created: 7/6/15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

//Takes in a number and determines what palindromic number it reaches and in how many steps (if at all)
//The step is to reverse the starting number and add them, ie 24 + 42 = 66 which is palindromic
namespace Easy_Challenge_218
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a number to make palindromic: ");
            long startNumber = long.Parse(Console.ReadLine());

            long steps = 0;

            BigInteger number = startNumber;
            char[] reverse = number.ToString().ToCharArray();
            Array.Reverse(reverse);
            string reversed = new string(reverse);
            BigInteger reversedNumber = BigInteger.Parse(reversed);

            while(number != reversedNumber && steps < long.MaxValue - 1)
            {
                number = number + reversedNumber;
                reverse = number.ToString().ToCharArray();
                Array.Reverse(reverse);
                reversed = new string(reverse);
                reversedNumber = BigInteger.Parse(reversed);

                steps++;
            }

            if (number == reversedNumber)
                Console.WriteLine(startNumber + " gets palindromic after " + steps + " steps: " + number);
            else
                Console.WriteLine("Error: either the number never becomes palindromic \n"
                    + "or it does not become palindromic after " + (long.MaxValue - 1) + "steps");

            Console.ReadLine();
        }
    }
}
