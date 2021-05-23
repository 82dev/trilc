using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trilc
{
    class Lexer
    {
        string source  = string.Empty;

        int start = 0;
        int current = 0;

        int line = 1;
        int col = 1;

        int lastLineIndex = 0;
        string lineStr{
            get => source.Substring(lastLineIndex, current-lastLineIndex);
        }

        List<Token> tokens = new List<Token>();

        static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            {"use"   ,   TokenType.USE},
            {"void"  ,   TokenType.VOID},
            {"int"   ,   TokenType.INT},
            {"bool"  ,   TokenType.BOOL},
            {"fn"    ,   TokenType.FN},
            {"if"    ,   TokenType.IF},
            {"else"  ,   TokenType.ELSE},
            {"while" ,   TokenType.WHILE},
            {"ensure",   TokenType.ENSURE},
        };

        public Lexer(string s)
        {
            source = s;
        }

        public Token[] lex(out bool error)
        {
            error = false;
            while (!isAtEnd())
            {
                try{
                    start = current;
                    scanToken();
                }
                catch(LexicalException){
                    error = true;
                }

            }
            addToken(TokenType.EOF, null);
            return tokens.ToArray();
        }

        bool isAtEnd() => current >= source.Length;
        char advance(){
            col++;
            return source[current++];
        }
        bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }
        char peek() => isAtEnd() ? '\0': source[current];

        void addToken(TokenType type)
        {
            tokens.Add(new Token(type, source.Substring(start, (current - start)),
                (short)line, (short)col,
            lineStr));
        }
        void addToken(TokenType type, string val)
        {
            tokens.Add(new Token(type, val,
                (short)line, (short)col,
            lineStr));
        }

        bool isDigit(char c) => ((c >= '0')&&(c <= '9'));

        bool isAlpha(char c) => ((c >= 'a' && c <= 'z') ||
                                (c >= 'A' && c <= 'Z') ||
                                c == '_');

        bool isAlphaNumeric(char c) => isAlpha(c) || isDigit(c);

        void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case ' ': case '\t': case '\r': break;

                case '\n':{
                    lastLineIndex = current;
                    col = 0;
                    line++;
                    break;
                }

                case ',':addToken(TokenType.Comma); break;
                case ';':addToken(TokenType.SemiColon);break;
                case ':':addToken(TokenType.Colon);break;
                case '+':{
                    if(match('=')){addToken(TokenType.PlusEqual);}
                    else if(match('+')){addToken(TokenType.Increase);}
                    else{
                        addToken(TokenType.Plus);
                    }
                    break;
                }
                case '-':{
                    if(match('=')){addToken(TokenType.MinusEqual);}
                    else if(match('-')){addToken(TokenType.Decrease);}
                    else{
                        addToken(TokenType.Minus);
                    }
                    break;
                }
                case '*':{addToken(
                        match('=')
                        ? TokenType.MulEqual
                        : TokenType.Asterisk
                        );
                        break;}
                case '/':{
                    if(match('=')){addToken(TokenType.SlashEqual);}
                    else if(match('/')){
                        while (peek() != '\n' && !isAtEnd()){
                            advance();
                        }
                    }
                    else{
                        addToken(TokenType.Slash);
                    }
                    break;
                }
                case '=':{addToken(
                        match('=')
                        ? TokenType.Equal
                        : TokenType.Assignment
                        );
                        break;}

                case '{':addToken(TokenType.BlockStart); break;
                case '}':addToken(TokenType.BlockEnd); break;

                case '[':addToken(TokenType.ArrSta); break;
                case ']':addToken(TokenType.ArrEnd); break;

                case '(':addToken(TokenType.ParSta); break;
                case ')':addToken(TokenType.ParEnd); break;

                case '!':{addToken(
                    match('=')
                    ? TokenType.NotEqual
                    : TokenType.Not
                ); break;}
                case '>':{
                    addToken(
                        match('=')
                        ? TokenType.GreaterEq
                        : TokenType.Greater
                    );
                    break;
                }
                case '<':{
                    addToken(
                        match('=')
                        ? TokenType.LesserEq
                        : TokenType.Lesser
                    );
                    break;
                }

                case '\"':{
                    String();
                    break;
                }

                default:
                    if(isDigit(c)){
                        num();
                    }
                    else if(isAlpha(c)){
                        id();
                    }
                    else{
                        error("Unexpected charecter.", c.ToString());
                    }
                    break;
            }
        }

        void String()
        {
            while (peek() != '"' && !isAtEnd())
            {
                if(peek() == '\n'){
                    error("Unclosed string character.", "\"");
                    line++;
                }
                advance();
            }
            if(isAtEnd()){error("Unclosed string character.", "\"");}
        }

        void num(){
            while (!isAtEnd() && isDigit(peek())){advance();}
            addToken(TokenType.NUM);
        }

        void id(){
            while(!isAtEnd() && isAlphaNumeric(peek())){advance();}
            string val = source.Substring(start, current - start);
            TokenType type = TokenType.ID;
            if(keywords.ContainsKey(val)){
                type = keywords[val];
            }
            addToken(type);
        }

        void error(string msg, string errorInducing, string lineStr = null, bool useCurrent = false){
            if(lineStr == null){ lineStr = this.lineStr;}

            string[] split = lineStr.Split(errorInducing);
            string indent = string.Empty;
            for (int i = 0;
                i <
                    (split[0].Length )+
                    (line.ToString().Length + 5)
                    ;
                i++)
            {
                indent += " ";
            }
             Error.formatStr($"{line }|    {split[0]}",
                            ConsoleColor.DarkRed, errorInducing,
                            null, split[1],
                            "\n",
                            ConsoleColor.Red,
                            // indent, "↑","\n",
                            msg,
                            "\n"
                        );
        }
    }
}