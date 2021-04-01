using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trilc
{
    class Token
    {
        public TokenType tokenType{get; private set;}
        public string value{get; private set;}

        public Token(TokenType tt, string v){
            this.tokenType = tt;
            this.value = v;
        }

        public Token(TokenType tt)
        {
            this.tokenType = tt;
            this.value = "";
        }

    }
}