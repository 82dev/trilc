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
        short lineIndex = 0;
        short charIndex = 0;

        private Dictionary<string, TokenType> stringTokenDict = new Dictionary<string, TokenType>(){
            {"use", TokenType.Use},
            {"true", TokenType.True},
            {"false", TokenType.False},
            {"null", TokenType.NULL},
            {"int", TokenType.INT},
            {"bool", TokenType.BOOL},
        };

        private List<Token> tokens = new List<Token>();
        private Dictionary<char, TokenType> charTokenDict = new Dictionary<char, TokenType>(){
            {'{', TokenType.BlockStart},
            {'}', TokenType.BlockEnd},
            {'[', TokenType.ArrSta},
            {']', TokenType.ArrEnd},
            {'(', TokenType.ParSta},
            {')', TokenType.ParEnd},
            {';', TokenType.SemiColon},
            {':', TokenType.Colon},
            {'*', TokenType.Asterisk},
            {'/', TokenType.Slash},
        };

        string seps = " =+/-*!{}[]().;:\'\"";
        string[] keywords = new string[]{};

        public Lexer(){}

        private void lex(string input)
        {
            string token = string.Empty;
            string state = string.Empty;

            for (charIndex = 0; charIndex < input.Length; charIndex++)
            {
                char peek(int j){
                    int t = charIndex; 
                    t+=j; 
                    return input[t];
                }                

                if(!seps.Contains(input[charIndex]))
                {
                    token += input[charIndex];
                }else
                {
                    if(token != string.Empty){
                        switch (token.ToUpper())
                        {
                            default:
                                if(stringTokenDict.ContainsKey(token)){
                                    addToken(stringTokenDict[token], token);
                                    break;
                                }

                                string temp = token;
                                if(temp.isNumber()){
                                    addToken(TokenType.NUM, token);
                                    break;
                                }
                                
                                if(!(string.IsNullOrEmpty(token)) ||
                                   !(string.IsNullOrWhiteSpace(token))){
                                    addToken(TokenType.ID, token);
                                }

                                break;
                        }
                        token = string.Empty;
                    }

                    switch (input[charIndex])
                    {
                        case '+': 
                            if(peek(1) == '+'){
                                charIndex++;
                                addToken(TokenType.Inc, input[charIndex]);
                                break;
                            }
                            addToken(TokenType.Plus, input[charIndex]);
                            break;
                        case '-': 
                            if(peek(1) == '-'){
                                charIndex++;
                                addToken(TokenType.Decrease, input[charIndex]);
                                break;
                            }
                            addToken(TokenType.Minus, input[charIndex]);
                            break;
                        case '.': 
                            if(peek(1) == '.'){
                                charIndex++;
                                goto Exit;
                                // addToken(TokenType.LineCom);
                                // break;
                            }
                            if(peek(1) == '['){
                                charIndex++;
                                addToken(TokenType.ComStart,input[charIndex]);
                                break;
                            }
                            addToken(TokenType.Dot,input[charIndex]);
                            break;
                        case ']': 
                            if(peek(1) == '.'){
                                charIndex++;
                                addToken(TokenType.ComEnd,input[charIndex]);
                                break;
                            }
                            addToken(TokenType.ArrEnd,input[charIndex]);
                            break;
                        case '=':
                            if(peek(1) == '='){
                                charIndex++;
                                addToken(TokenType.Equal,input[charIndex]);
                                break;
                            }
                            addToken(TokenType.Assignment,input[charIndex]);
                            break;
                        case '!':
                            if(peek(1) == '='){
                                charIndex++;
                                addToken(TokenType.NotEqual,input[charIndex]);
                                break;
                            }
                            addToken(TokenType.Not,input[charIndex]);
                            break;
                        case '\"': 
                            string temp = string.Empty;
                            charIndex++;
                            while (input[charIndex] != '\"')
                            {
                                temp += input[charIndex];
                                charIndex++;
                            }
                            addToken(TokenType.String, temp);
                            temp = string.Empty;
                            break;
                        default:
                            if(charTokenDict.ContainsKey(input[charIndex])){
                                addToken(charTokenDict[input[charIndex]],input[charIndex]);
                            }
                            break;
                    }
                }

                if(tokens.LastOrDefault().tokenType == TokenType.LineCom){
                    state = "linecomment";
                }
            
            }
            
            Exit:;
        }

        public Token[] lex(string[] input){
            for (lineIndex = 0; lineIndex < input.Length; lineIndex++)
            {
                this.lex(input[lineIndex] + " ");
            }
            addToken(TokenType.EOF, "EOF");
            return tokens.ToArray();
        }

        private void addToken(TokenType tokenType, char v){
            tokens.Add(new Token(tokenType, v.ToString(), (short)(lineIndex + 1), (short)(charIndex+1)));
        }
        private void addToken(TokenType tokenType, string v){
            tokens.Add(new Token(tokenType, v, (short)(lineIndex + 1), (short)(charIndex+1)));
        }
        private void addToken(TokenType tokenType){
            tokens.Add(new Token(tokenType, (short)(lineIndex + 1), (short)(charIndex+1)));
        }

    }
}