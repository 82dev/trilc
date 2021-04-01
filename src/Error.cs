namespace trilc
{
    using System;

    class Error
    {
        public Error(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error! {str}");
            Console.ResetColor();
        }
    }
}