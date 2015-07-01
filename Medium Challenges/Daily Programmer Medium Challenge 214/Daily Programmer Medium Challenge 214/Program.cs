//Author: Hunter Green
//Created: 6/24/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Programmer_Medium_Challenge_214
{
    class Program
    {
        static void Main(string[] args)
        {
            //Input the canvas
            Console.WriteLine("Enter the canvas dimensions: ");
            string[] dimensions = Console.ReadLine().Split(' ');
            int dimX = Int32.Parse(dimensions[0]);
            int dimY = Int32.Parse(dimensions[1]);
            int[,] canvas = new int[dimX, dimY];

            List<int> colors = new List<int>();
            colors.Add(0);

            //Input the notes
            Console.WriteLine("Enter the note color, top-left corner position, and dimensions seperated by spaces: (Press Q to finish input)");
            string note = Console.ReadLine();
            while (note[0] != 'Q' && note[0] != 'q')
            {
                string[] noteData = note.Split(' ');
                int color = Int32.Parse(noteData[0]);
                if (!colors.Contains(color))
                    colors.Add(color);

                int topLeftX = Int32.Parse(noteData[1]);
                int topLeftY = Int32.Parse(noteData[2]);
                int noteDimX = Int32.Parse(noteData[3]);
                int noteDimY = Int32.Parse(noteData[4]);

                for (int x = topLeftX; x < topLeftX + noteDimX; x++)
                {
                    for (int y = topLeftY; y < topLeftY + noteDimY; y++)
                    {
                        canvas[x, y] = color;
                    }
                }

                note = Console.ReadLine();
            }

            //Calculate the total area of each color
            Dictionary<int, int> colorCounts = new Dictionary<int, int>();
            foreach (int n in colors)
            {
                colorCounts.Add(n, 0);
            }

            foreach (int n in canvas)
            {
                colorCounts[n]++;
            }

            //Print that total
            for (int i = 0; i < colors.Count; i++)
            {
                Console.WriteLine("Color " + colors[i] + " has " + colorCounts[i] + " area");
            }

            Console.ReadLine();
        }
    }
}
