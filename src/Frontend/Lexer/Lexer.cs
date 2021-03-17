using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrilComp;

namespace TrilComp
{
    class Lexer
    {
        private List<Token> tokens = new List<Token>();

        string seps = " =+\\-*!{}[]().";

        public Lexer(){}

        private void lex(string input)
        {
            string token = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                char peek(int j){
                    int t = i; 
                    t+=j; 
                    return input[t];
                }

                if(!seps.Contains(input[i]))
                {
                    token += input[i];
                }else
                {
                    if(token != string.Empty){
                        switch (token.ToUpper())
                        {
                            default:
                                string temp = token;
                                if(temp.isNumber()){
                                    tokens.Add(new Token(TokenType.Number, token));
                                    break;
                                }
                                
                                if(!(string.IsNullOrEmpty(token)) ||
                                   !(string.IsNullOrWhiteSpace(token))){

                                    tokens.Add(new Token(TokenType.Indentifier, token));
                                }

                                break;
                        }
                        token = string.Empty;
                    }

                    switch (input[i])
                    {
                        case '{': 
                            tokens.Add(new Token(TokenType.BlockStart));
                            break;
                        case '}': 
                            tokens.Add(new Token(TokenType.BlockEnd));
                            break;
                        case '+': 
                            if(peek(1) == '+'){
                                i++;
                                tokens.Add(new Token(TokenType.Inc));
                                break;
                            }
                            tokens.Add(new Token(TokenType.Plus));
                            break;
                        case '-': 
                            if(peek(1) == '-'){
                                i++;
                                tokens.Add(new Token(TokenType.Decrease));
                                break;
                            }
                            tokens.Add(new Token(TokenType.Minus));
                            break;
                        case '.': 
                            if(peek(1) == '.'){
                                i++;
                                tokens.Add(new Token(TokenType.LineCom));
                                break;
                            }
                            if(peek(1) == '['){
                                i++;
                                tokens.Add(new Token(TokenType.ComStart));
                                break;
                            }
                            tokens.Add(new Token(TokenType.Dot));
                            break;
                        case ']': 
                            if(peek(1) == '.'){
                                i++;
                                tokens.Add(new Token(TokenType.ComEnd));
                                break;
                            }
                            break;
                        default:break;
                    }
                }
            }
        }

        public Token[] lex(string[] input){
            for (int i = 0; i < input.Length; i++)
            {
                this.lex(input[i] + " ");
            }
            return tokens.ToArray();
        }

    }

    public static class Extensions{
        public static bool isNumber(this String input){
            foreach (var item in input)
            {
                if(!char.IsDigit(item)){
                    return false;
                }
            }
            return true;
        }
    }
}