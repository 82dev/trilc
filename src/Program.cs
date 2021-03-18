using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrilComp
{
    class Program
    {
        static void Main(string[] args)
        {
            Add add = new Add();

            // string[] input = File.ReadAllLines(Path.Join(Environment.CurrentDirectory, 
            //     @""))

            string path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).ToString(), @"grammar\example.tril");
            path = Path.Combine(Environment.CurrentDirectory, @"test.tril");

            Lexer lexer = new Lexer();

            // foreach(var item in lexer.lex(File.ReadAllLines(path))){
            //     Console.Write("Type: " + item.tokenType);
            //     if(!string.IsNullOrEmpty(item.value)){
            //         Console.Write(" Value: " + item.value);
            //     }
            //     Console.Write("\n");
            // }

            Expression exp = new Expression(new Expression(2, 3, add), new Expression(12, 3, add), add);

            Console.WriteLine(exp.value);

            Console.ReadLine();
        }
    }
}
