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

        private Dictionary<char, TokenType> charTokenDict = new Dictionary<char, TokenType>(){
            {'{', TokenType.BlockStart},
            {'}', TokenType.BlockEnd},
            {'[', TokenType.ArrSta},
            {']', TokenType.ArrEnd},
            {'(', TokenType.ParSta},
            {')', TokenType.ParEnd},
            {';', TokenType.SemiColon},
        };

        string seps = " =+\\-*!{}[]().;";
        string[] consts = new string[]{"null","string","int"};

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
                                    addToken(TokenType.NUM, token);
                                    break;
                                }
                                
                                if(!(string.IsNullOrEmpty(token)) ||
                                   !(string.IsNullOrWhiteSpace(token))){

                                    if(consts.Contains(token)){
                                        addToken(TokenType.CONST, token);
                                        break;
                                    }

                                    addToken(TokenType.ID, token);
                                }

                                break;
                        }
                        token = string.Empty;
                    }

                    switch (input[i])
                    {
                        case '+': 
                            if(peek(1) == '+'){
                                i++;
                                addToken(TokenType.Inc);
                                break;
                            }
                            addToken(TokenType.Plus);
                            break;
                        case '-': 
                            if(peek(1) == '-'){
                                i++;
                                addToken(TokenType.Decrease);
                                break;
                            }
                            addToken(TokenType.Minus);
                            break;
                        case '.': 
                            if(peek(1) == '.'){
                                i++;
                                addToken(TokenType.LineCom);
                                break;
                            }
                            if(peek(1) == '['){
                                i++;
                                addToken(TokenType.ComStart);
                                break;
                            }
                            addToken(TokenType.Dot);
                            break;
                        case ']': 
                            if(peek(1) == '.'){
                                i++;
                                addToken(TokenType.ComEnd);
                                break;
                            }
                            addToken(TokenType.ArrEnd);
                            break;
                        default:
                            if(charTokenDict.ContainsKey(input[i])){
                                addToken(charTokenDict[input[i]]);
                            }
                            break;
                    }
                }
            }
        }

        public Token[] lex(string[] input){
            for (int i = 0; i < input.Length; i++)
            {
                this.lex(input[i] + " ");
            }
            addToken(TokenType.EOF);
            return tokens.ToArray();
        }

        private void addToken(TokenType tokenType, string v){
            tokens.Add(new Token(tokenType, v));
        }
        private void addToken(TokenType tokenType){
            tokens.Add(new Token(tokenType));
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