using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace trilc
{
    class Parser
    {
        public AST parse(Token[] tokens){
            string state = string.Empty;
            for (int i = 0; i < tokens.Length; i++)
            {
                Token token = tokens[i];

                // if(token.tokenType == TokenType.EOL){
                //     state = string.Empty;
                //     temp.RemoveRange(1,tmp);
                // }
                // if(token.tokenType == TokenType.LineCom){
                //     state = "comment";
                // }
                // if(state == "comment"){
                //     tmp++;
                // }
                
            }

            return new AST();
        }
    }
}