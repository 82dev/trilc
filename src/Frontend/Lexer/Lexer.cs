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
            {"use", TokenType.Use},
            {"true", TokenType.True},
            {"false", TokenType.False},
            {"fn", TokenType.Func},
            {"void", TokenType.VOID},
            {"int", TokenType.INT},
            {"bool", TokenType.BOOL},
        };
        static char[] seps = " =+/-*!{}[]().;:\'\"<>\n".ToCharArray();
        static Dictionary<char, TokenType> charTokenDict = new Dictionary<char, TokenType>(){
            {'{', TokenType.BlockStart},
            {'}', TokenType.BlockEnd},
            {'[', TokenType.ArrSta},
            {']', TokenType.ArrEnd},
            {'(', TokenType.ParSta},
            {')', TokenType.ParEnd},
            {';', TokenType.SemiColon},
            {':', TokenType.Colon},
            {'*', TokenType.Asterisk},
            {'.', TokenType.Dot},
        };

        int lineIndex = 0;
        short charIndex = 0;
        List<Token> tokens = new List<Token>();

        public Token[] lex(string source){
            string buffer = string.Empty;

            int i = 0;

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
                            lineIndex++;
                            charIndex = 0;
                            break;
                        }
                        case '+':{ 
                            if(expect(1, '+')){
                                addToken(TokenType.Increase, "++");
                            }
                            addToken(TokenType.Plus);
                            break;
                        }
                        case '-':{ 
                            if(expect(1, '-')){
                                addToken(TokenType.Decrease, "--");
                            }
                            addToken(TokenType.Minus);
                            break;
                        }
                        case '=':{
                            if(expect(1, '=')){
                                addToken(TokenType.Equal, "==");
                            }
                            addToken(TokenType.Assignment, "=");
                            break;
                        }
                        case '/':{
                            if(expect(1, '/')){
                                int tmp = i;
                                while (source[tmp] != '\n' || tmp >= source.Length)
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
                            while (source[j] != '\"')
                            {
                                tmp += source[j];
                                j++;
                            }
                            i = j;
                            addToken(TokenType.String, tmp);
                            break;
                        }
                        case '>':{
                            if(expect(1, '=')){
                                addToken(TokenType.GreaterEq, ">=");
                            }
                            addToken(TokenType.Greater);
                            break;
                        }
                        case '<':{
                            if(expect(1, '=')){
                                addToken(TokenType.LesserEq, "<=");
                            }
                            addToken(TokenType.Lesser);
                            break;
                        }
                        case '!':{
                            if(expect(1, '=')){
                                addToken(TokenType.NotEqual, ">=");
                            }
                            addToken(TokenType.Not);
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
                i++;
            }

            addToken(TokenType.EOF, null);
            return tokens.ToArray();
        }

        #region AddToken
            private void addToken(TokenType tokenType, char v){
                tokens.Add(new Token(tokenType, v.ToString(), (Int16)(lineIndex + 1), (short)(charIndex+1)));
            }
            private void addToken(TokenType tokenType, string v){
                tokens.Add(new Token(tokenType, v, (Int16)(lineIndex + 1), (short)(charIndex+1)));
            }
            private void addToken(TokenType tokenType){
                tokens.Add(new Token(tokenType, (Int16)(lineIndex + 1), (short)(charIndex+1)));
            }
        #endregion
    }
}//nice