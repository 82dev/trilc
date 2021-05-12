using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trilc
{
    public class Token
    {
        public TokenType tokenType{get; private set;}
        public string value{get; private set;}
        public Int16 lineIndex{get; private set;}
        public Int16 charIndex{get; private set;}

        public Token(TokenType tt, string v, Int16 i, Int16 c){
            this.tokenType = tt;
            this.value = v;
            this.lineIndex = i;
            this.charIndex = c;
        }

        public Token(TokenType tt, Int16 i, Int16 c)
        {
            this.tokenType = tt;
            this.value = null;
            this.lineIndex = i;
            this.charIndex = c;
        }

        public override string ToString()
        {
            if(value != null){
                //return this.value + $"  Type: {Enum.GetName(typeof(TokenType), tokenType)}";
                return this.value;
            }
            if(tokenType == TokenType.EOF){
                return "EOF";
            }
            return $"Type: {Enum.GetName(typeof(TokenType), tokenType)}";
        }
    }
}