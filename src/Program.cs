using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace trilc
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();

            if(args[0] != null){
                string json = File.ReadAllText(args[0]);
                metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            else
            {
                System.Console.WriteLine("Correct usage: trilc 'path to file'");
                Environment.Exit(0);
            }

            // Parser parser = new Parser(File.ReadAllText(Directory.GetParent(args[0]).ToString()
            //                                                                  + "\\" 
            //                                                                  + metadata["entryfile"]
            //                                                                  + ".tril"));

            Parser parser = new Parser(File.ReadAllLines(
                                        Directory.GetParent(args[0]).ToString()
                                                                            + "\\"
                                                                            + metadata["entryfile"]
                                                                            + ".tril"
                                        ));

            parser.parse();
            // int i = 1;
            // foreach (var item in ast.root.children)
            // {
            //     int j = i;
            //     foreach (var ite in item)
            //     {
            //         string space = string.Empty;
            //         int k = j * 4;
            //         while (k > 0)
            //         {
            //             space += " ";
            //             k--;
            //         }
            //         System.Console.WriteLine(space + ite.GetType().ToString());
            //     }
            //     i++;
            // }


            // foreach(var item in lexer.lex(File.ReadAllLines(path))){
            //     Console.Write("Type: " + item.tokenType);
            //     if(!string.IsNullOrEmpty(item.value)){
            //         Console.Write(" Value: " + item.value);
            //     }
            //     Console.Write("\n");
            // }
            Console.ReadLine();
        }
    }
}