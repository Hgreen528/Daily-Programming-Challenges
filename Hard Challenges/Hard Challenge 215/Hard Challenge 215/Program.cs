//Author: Hunter Green
//Created: 6/26/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hard_Challenge_215
{
    class Program
    {
        static void Main(string[] args)
        {
            //Metaprogramming: programming programming programs
            Console.WriteLine("{0, -15} {1, -1} {2}", "Expression", "|", "Bool");
            Console.WriteLine("----------------+-------------------------------");
            Console.WriteLine("{0, -15} {1, -1} {2}", "\"Hello World!\"", "|", evaluateTruthness<string>("Hello World!"));
            Console.WriteLine("{0, -15} {1, -1} {2}", "\"\" ", "|", evaluateTruthness<string>(""));
            Console.WriteLine("{0, -15} {1, -1} {2}", "'0' ", "|", evaluateTruthness<char>('0'));
            Console.WriteLine("{0, -15} {1, -1} {2}", "null ", "|", evaluateTruthness<object>(null));
            Console.WriteLine("{0, -15} {1, -1} {2}", "0b ", "|", evaluateTruthness<byte>(0));
            Console.WriteLine("{0, -15} {1, -1} {2}", "1 ", "|", evaluateTruthness<int>(1));
            Console.WriteLine("{0, -15} {1, -1} {2}", "0 ", "|", evaluateTruthness<int>(0));
            Console.WriteLine("{0, -15} {1, -1} {2}", "0.0f ", "|", evaluateTruthness<float>(0.0f));
            Console.WriteLine("{0, -15} {1, -1} {2}", "[] ", "|", evaluateTruthness<int[]>(new int[0]));
            Console.WriteLine("{0, -15} {1, -1} {2}", "[1, 2, 3] ", "|", evaluateTruthness<int[]>(new int[3] { 1, 2, 3 }));
            Console.WriteLine("{0, -15} {1, -1} {2}", "True ", "|", evaluateTruthness<bool>(true));
            Console.WriteLine("{0, -15} {1, -1} {2}", "False ", "|", evaluateTruthness<bool>(false));
            Console.ReadLine();
        }

        static string evaluateTruthness<T>(T val)
        {
            try
            {
                bool truthness = Convert.ToBoolean(val);
                return truthness.ToString();
            }
            catch (InvalidCastException)
            {
                return "Not evaluatable to boolean";
            }
            catch (FormatException)
            {
                return "Not evaluatable to boolean";
            }
        }
    }
}
