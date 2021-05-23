﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using fastJSON;
using System.Diagnostics;

namespace trilc
{
    class Program
    {
        static int Main(string[] args)
        {
            bool hadError = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            int exit = 0;

            #region MD
            //get the metadata required from the .tmd
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            if(args[0] != null){
                string json = File.ReadAllText(Path.Join(args[0], ".tmd"));
                // metadata = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                metadata = (Dictionary<string , object>) fastJSON.JSON.Parse(json);
                stopwatch.Stop();
                Debug.assert($"Processed json in {stopwatch.ElapsedMilliseconds}ms");
                stopwatch = Stopwatch.StartNew();
            }
            else
            {
                System.Console.WriteLine("Correct usage: trilc 'path to folder with Tril project'");
                System.Environment.Exit(0);
            }

            if(!metadata.ContainsKey("entryfile")){
                System.Console.WriteLine("entryfile not provided defualting to main.tril");
                metadata.Add("entryfile", "main");
            }

            if(metadata.ContainsKey("deps")){
                var arr = (List<object>)metadata["deps"];
                List<string> t = new List<string>();

                foreach (var item in arr)
                {
                    t.Add(item.ToString());
                }
            }

            string fileText;

            try
            {
                fileText = File.ReadAllText(args[0]
                                        + "\\"
                                        + metadata["entryfile"]
                                        + ".tril");
            }
            catch (FileNotFoundException)
            {
                System.Console.WriteLine($"The entryfile in '"+Path.Join(args[0], ".tmd")+"' is not a valid file. Compilation terminated");
                exit = -1;
                goto Done;
            } 

            string flags = "-n";
            if(args[1] != null){
                flags = args[1];
            }
            #endregion

            Token[] tokens = new Lexer(fileText).lex(out hadError);
            stopwatch.Stop();
            Debug.assert($"Lexer finished in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch = Stopwatch.StartNew();

            Parser parser = new Parser(tokens);
            var AST = parser.parse();
            stopwatch.Stop();
            Debug.assert($"Parser finished in {stopwatch.ElapsedMilliseconds}ms");

            var semanticChecker = new SemanticChecker();
            semanticChecker.Check(AST.children, ref hadError);

            if(!hadError){
                var codeGen = new CodeGen();
                Directory.CreateDirectory(Path.Join(args[0], "\\bin"));
                File.Create(Path.Join(args[0], "\\bin\\output.tco")).Close();
                Writer.write((Path.Join(args[0], "\\bin\\output.tco")),codeGen.genProg(AST));
            }

            Done:;  
            #if DEBUG
                Console.ReadLine();
            #endif
            return exit;
        }
    }
}