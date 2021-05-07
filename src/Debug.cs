using System;

namespace trilc
{
    class Debug
    {
        public static void assert(string str){
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(str);
            Console.ResetColor();
        }
    }
}