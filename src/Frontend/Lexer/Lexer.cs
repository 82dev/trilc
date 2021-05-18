using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using trilc;

namespace trilc
{
    class Lexer
    {
        static Dictionary<string, TokenType> stringTokenDict = new Dictionary<string, TokenType>(){
            {"use", TokenType.USE},
            {"true", TokenType.TRUE},
            {"false", TokenType.FALSE},
            {"fn", TokenType.FN},
            {"void", TokenType.VOID},
            {"int", TokenType.INT},
            {"bool", TokenType.BOOL},
            {"ensure", TokenType.ENSURE},
            {"if", TokenType.IF},
            {"else", TokenType.ELSE},
            {"while", TokenType.WHILE},
        };
        static char[] seps = " =+/-*!{}[](),;:\'\"\n".ToCharArray();
        static Dictionary<char, TokenType> charTokenDict = new Dictionary<char, TokenType>(){
            {'{', TokenType.BlockStart},
            {'}', TokenType.BlockEnd},
            {'[', TokenType.ArrSta},
            {']', TokenType.ArrEnd},
            {'(', TokenType.ParSta},
            {')', TokenType.ParEnd},
            {';', TokenType.SemiColon},
            {':', TokenType.Colon},
            {',', TokenType.Comma},
        };

        int lineIndex = 0;
        short charIndex = 0;
        string[] lines;
        List<Token> tokens = new List<Token>();

        public Token[] lex(string source){
            string buffer = string.Empty;
            lines = source.Split('\n');
            int i = 0;

            void newline(){
                lineIndex++;
                charIndex = 0;
            }

            char peek(int j) => source[i + 1];
            bool expect(int j, char c){
                if(peek(j) == c){
                    i += j;
                    return true;
                }
                return false;
            }

            while (i < source.Length)
            {
                charIndex = (short)i;
                if(!seps.Contains(source[i])){
                    buffer += source[i];
                }
                else{
                    if(buffer.Replace("\n", "").Replace("\r","") != string.Empty){
                        switch (buffer)
                        {
                            default:
                                if(stringTokenDict.ContainsKey(buffer)){
                                    addToken(stringTokenDict[buffer], buffer);
                                    break;
                                }

                                string temp = buffer;
                                if(temp.isNumber()){
                                    addToken(TokenType.NUM, buffer);
                                    break;
                                }
                                
                                if(!(string.IsNullOrEmpty(buffer)) ||
                                   !(string.IsNullOrWhiteSpace(buffer))){
                                    addToken(TokenType.ID, buffer);
                                }

                                break;
                        }
                        buffer = string.Empty;
                    }

                    switch (source[i])
                    {
                        case '\n':{
                            newline();
                            break;
                        }
                        case '+':{
                            if(expect(1, '=')){
                                addToken(TokenType.PlusEqual, "+=");
                                break;
                            }
                            if(expect(1, '+')){
                                addToken(TokenType.Increase, "++");
                                break;
                            }
                            addToken(TokenType.Plus, "+");
                            break;
                        }
                        case '-':{ 
                            if(expect(1, '=')){
                                addToken(TokenType.MinusEqual, "-=");
                                break;
                            }
                            if(expect(1, '-')){
                                addToken(TokenType.Decrease, "--");
                                break;
                            }
                            addToken(TokenType.Minus, "-");
                            break;
                        }
                        case '=':{
                            if(expect(1, '=')){
                                addToken(TokenType.Equal, "==");
                                break;
                            }
                            addToken(TokenType.Assignment, "=");
                            break;
                        }
                        case '*':{
                            if(expect(1, '=')){
                                addToken(TokenType.MulEqual, "*=");
                                break;
                            }
                            addToken(TokenType.Asterisk, "*");
                            break;
                        }
                        case '/':{
                            if(expect(1, '=')){
                                addToken(TokenType.SlashEqual, "/=");
                                break;
                            }
                            if(expect(1, '/')){
                                int tmp = i;
                                while (tmp < source.Length && source[tmp] != '\n')
                                {
                                    tmp++;
                                }
                                i = tmp;
                                break;
                            }
                            addToken(TokenType.Slash, "/");
                            break;
                        }
                        case '\"':{
                            string tmp = string.Empty;
                            int j = i+1;

                            void err(){
                                string[] qouteSplit = lines[lineIndex - 1].Split('\"');
                                string indent = string.Empty;
                                for (int k = 0;
                                    k < 
                                        (qouteSplit[0].Length )+
                                        (lineIndex.ToString().Length + 5)
                                        ;
                                    k++)
                                {
                                    indent += " ";
                                }
                                Error.formatStr($"{lineIndex }|    {qouteSplit[0]}",
                                                ConsoleColor.DarkRed, "\"",
                                                null, qouteSplit[1],
                                                "\n",
                                                ConsoleColor.Red,
                                                indent, "↑","\n",
                                                "Unclosed string here.",
                                                "\n"
                                            );
                            }

                            try
                            {
                                while ((source[j] != '\"'))
                                {
                                    if(source[j] == '\n'){
                                        newline();
                                        err();
                                        goto Done;
                                    }
                                    tmp += source[j];
                                    j++;
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {
                                newline();
                                err();
                                goto Done;
                            }
                                
                            i = j;
                            addToken(TokenType.String, tmp);
                            break;
                        }
                        case '>':{
                            if(expect(1, '=')){
                                addToken(TokenType.GreaterEq, ">=");
                                break;
                            }
                            addToken(TokenType.Greater, ">");
                            break;
                        }
                        case '<':{
                            if(expect(1, '=')){
                                addToken(TokenType.LesserEq, "<=");
                                break;
                            }
                            addToken(TokenType.Lesser, "<");
                            break;
                        }
                        case '!':{
                            if(expect(1, '=')){
                                addToken(TokenType.NotEqual, "!=");
                                break;
                            }
                            addToken(TokenType.Not, "!");
                            break;
                        }
                        case '&':{
                            if(expect(1, '&')){
                                addToken(TokenType.And, "&&");
                            }
                            break;    
                        }
                        case '|':{
                            if(expect(1, '|')){
                                addToken(TokenType.Or, "||");
                            }
                            break;
                        }

                        default:{
                            if(charTokenDict.ContainsKey(source[i])){
                                addToken(charTokenDict[source[i]], source[i]);
                                break;
                            }
                            
                            break;
                        }
                    }

                    buffer = string.Empty;
                }
                
                Done:;
                i++;
            }

            if(buffer.Replace("\n", "").Replace("\r","") != string.Empty){
                switch (buffer)
                {
                    default:
                        if(stringTokenDict.ContainsKey(buffer)){
                            addToken(stringTokenDict[buffer], buffer);
                            break;
                        }

                        string temp = buffer;
                        if(temp.isNumber()){
                            addToken(TokenType.NUM, buffer);
                            break;
                        }
                        
                        if(!(string.IsNullOrEmpty(buffer)) ||
                            !(string.IsNullOrWhiteSpace(buffer))){
                            addToken(TokenType.ID, buffer);
                        }

                        break;
                }
                buffer = string.Empty;
            }

            addToken(TokenType.EOF, null);
            return tokens.ToArray();
        }

        #region AddToken
            private void addToken(TokenType tokenType, char v){
                tokens.Add(new Token(tokenType, v.ToString(), (Int16)(lineIndex + 1), (short)(charIndex+1), lines[lineIndex != 0 ?(lineIndex-1): 0]));
            }
            private void addToken(TokenType tokenType, string v){
                tokens.Add(new Token(tokenType, v, (Int16)(lineIndex + 1), (short)(charIndex+1), lines[lineIndex != 0 ?(lineIndex-1): 0]));
            }
            // private void addToken(TokenType tokenType){
            //     tokens.Add(new Token(tokenType, (Int16)(lineIndex + 1), (short)(charIndex+1)));
            // }
        #endregion
    }
}