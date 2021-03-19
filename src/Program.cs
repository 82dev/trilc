using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrilComp
{
    class Program
    {
        static void Main(string[] args)
        {
            Add add = new Add();
            Sub sub = new Sub();
            Mul mul = new Mul();
            Div div = new Div();

            string path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).ToString(), @"grammar\example.tril");
            path = Path.Combine(Environment.CurrentDirectory, @"test.tril");

            Lexer lexer = new Lexer();
            lexer.lex(File.ReadAllLines(path));

            // foreach(var item in lexer.lex(File.ReadAllLines(path))){
            //     Console.Write("Type: " + item.tokenType);
            //     if(!string.IsNullOrEmpty(item.value)){
            //         Console.Write(" Value: " + item.value);
            //     }
            //     Console.Write("\n");
            // }

            if(args[0] != null){
                var jsonS = File.ReadAllText(args[0]);
                var options = new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                };
                TrilMetaData e = JsonSerializer.Deserialize<TrilMetaData>("\"entryfile\":\"program\",\"entryclass\":\"none\"", options);
            }

            Console.ReadLine();
        }
    }
}
