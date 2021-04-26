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
            //get the metadat required from the .tmd
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            if(args[0] != null){
                string json = File.ReadAllText(Path.Join(args[0], ".tmd"));
                metadata = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            else
            {
                System.Console.WriteLine("Correct usage: trilc 'path to folder with Tril project'");
                Environment.Exit(0);
            }

            if(!metadata.ContainsKey("entryfile")){
                System.Console.WriteLine("entryfile not provided defualting to main.tril");
                metadata.Add("entryfile", "main");
            }

            if(metadata.ContainsKey("deps")){
                Newtonsoft.Json.Linq.JArray arr = (Newtonsoft.Json.Linq.JArray)metadata["deps"];
                List<string> t = new List<string>();
                foreach (var item in arr)
                {
                    t.Add(item.ToString());
                }
            }

            string[] fileLines;

            try
            {
                fileLines = File.ReadAllLines(args[0]
                                        + "\\"
                                        + metadata["entryfile"]
                                        + ".tril");
            }
            catch (FileNotFoundException)
            {
                System.Console.WriteLine($"The entryfile in '"+Path.Join(args[0], ".tmd")+"' is not a valid file. Compilation terminated");
                goto Done;
            } 

            string flags = "-n";
            if(args[1] != null){
                flags = args[1];
            }

            Parser parser = new Parser(fileLines);
            var prog = parser.parse();

            TypeChecker.CheckBlock(prog);

            Done:;
            #if DEBUG
                Console.ReadLine();
            #endif
        }
    }
}