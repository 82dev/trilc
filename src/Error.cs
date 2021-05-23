namespace trilc
{
    using System;

    class Error
    {
        public static void formatStr(params object[] objs)
        {
            foreach(var o in objs){
                if(o == null){
                    Console.ResetColor();
                }
                else if(o is ConsoleColor cc){
                    Console.ForegroundColor = cc;
                }
                else{
                    Console.Write(o.ToString());
                }
            }
        }

        public static void assert(string str){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ResetColor();
        }
    }

    [System.Serializable]
    public class ParseException : System.Exception
    {
        public ParseException() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, System.Exception inner) : base(message, inner) { }
        protected ParseException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class SemanticException : System.Exception
    {
        public SemanticException() { }
        public SemanticException(string message) : base(message) { }
        public SemanticException(string message, System.Exception inner) : base(message, inner) { }
        protected SemanticException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class LexicalException : System.Exception
    {
        public LexicalException() { }
        public LexicalException(string message) : base(message) { }
        public LexicalException(string message, System.Exception inner) : base(message, inner) { }
        protected LexicalException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}