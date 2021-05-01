using System;
using System.Collections.Generic;

namespace trilc
{
    class Parser
    {
        Token cur{get => tokens[index];}
        int index = 0;
        Token[] tokens;

        bool isEOF() => cur.tokenType == TokenType.EOF;
        Token peek(int i) => tokens[index + i];
        Token previous() => peek(-1);

        Token advance(){
            if(!isEOF()){
                index++;
            }
            return previous();
        }

        bool match(params TokenType[] token){
            foreach (var item in token)
            {
                if(check(item)){
                    advance();
                    return true;
                }
            }
            return false;
        }

        bool check(TokenType type){
            if(isEOF()){
                return false;
            }
            return cur.tokenType == type;
        }

        public Parser(string fileText)
        {
            tokens = new Lexer().lex(fileText);
        }

        public Stmt.Program parse(){
            return program();
        }

        Stmt.Program program()
        {
            List<Stmt> stmts = new List<Stmt>();
            while (!match(TokenType.EOF))
            {
                stmts.Add(stmt());
            }
            return new Stmt.Program(stmts);
        }

        // Stmt.Block block(){

        // }

        Stmt stmt(){
            return null;
        }
    }
}