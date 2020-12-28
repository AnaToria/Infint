using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgAssignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            // defining .txt file's path and reading each line from it
            string filePath = @"C:\Users\anano\Desktop\infint.txt";
            string[] lines = File.ReadAllLines(filePath);

            // Using the fact that pattern is multiple of 3 
            for (int i = 0; i < lines.Length; i += 3)
            {
                InfInt num1 = new InfInt(lines[i]);
                InfInt num2 = new InfInt(lines[i + 1]);
                string opr = new string(lines[i + 2]);
                Console.WriteLine(lines[i] + " " + lines[i + 2] + " " + lines[i + 1] + " = ");
                num1.Result(num2, opr);
                Console.WriteLine();
            }

            Console.ReadLine();

        }
    }
}