using System;
using System.Collections.Generic;
using static trilc.TokenType;
using static trilc.Stmt;

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

        private void synchronize(){
            advance();
            while (cur.tokenType != TokenType.EOF)
            {
                if(previous().tokenType == TokenType.SemiColon) return;
                
                // switch (cur.tokenType)
                // {
                    // case TokenType.CLASS:
                    // case TokenType.FUN:
                    // case TokenType.VAR:
                    // case TokenType.FOR:
                    // case TokenType.IF:
                    // case TokenType.WHILE:
                    // case TokenType.PRINT:
                    // case TokenType.RETURN:
                    //     return;
                // }
                advance();
            }
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
        bool expect(string error, params TokenType[] token){
            if(!match(token)){
                Error.assert(error);
                throw new ParseException();
            }
            return true;
        }

        ParseException error(string err){
            Error.assert(err);
            return new ParseException();
        }

        public Parser(Token[] t)
        {
            tokens = t;
        }

        #region Stmts
        public Stmt.Program parse(){
            return program();
        }

        Stmt.Program program()
        {
            List<Stmt> stmts = new List<Stmt>();
            while (!isEOF())
            {
                var a = stmt();
                if(a != null){
                    stmts.Add(a);
                }
            }
            return new Stmt.Program(stmts);
        }

        Stmt.Block block(){
            List<Stmt> stmts = new List<Stmt>();
            while (!match(TokenType.BlockEnd))
            {
                stmts.Add(stmt());
            }
            return new Stmt.Block(stmts);
        }

        Stmt stmt(){
            try
            {
                if(newVar()){
                    return newAss();
                }
                if(match(TokenType.BlockStart)){
                    return block();
                }
                if(match(TokenType.SemiColon)){
                    return new Stmt.Empty();
                }
            }
            catch (ParseException)
            {
                synchronize();
            }
            return null;
        }

        Stmt.newAssign newAss(){
            string n = peek(-3).value;
            Token t = previous();
            expect($"Expect '=' after '{n}:{t}'",TokenType.Assignment);
            var e = expr();
            expect("Expect ';'!", TokenType.SemiColon);
            return new Stmt.newAssign(n,e,t);
        }

        #region Expressions
        Stmt.Expr expr(){
            return equality();
        }

        Stmt.Expr equality(){
            Expr expr = comparision();
            while (match(TokenType.NotEqual, TokenType.Equal)){
                var op = previous();
                expr = new Expr.Binary(expr, op, comparision());
            }
            return expr;
        }

        Expr comparision() {
            Expr expr = term();
            while (match(Greater, GreaterEq, Lesser, LesserEq)) {
                Token op = previous();
                expr = new Expr.Binary(expr, op, term());
            }
            return expr;
        }

        private Expr term()
        {
            Expr expr = factor();
            while (match(Plus, Minus)){
                Token op = previous();
                expr = new Expr.Binary(expr, op, factor());
            }
            return expr;
        }

        private Expr factor()
        {
            Expr expr = unary();
            while (match(Asterisk, Slash)){
                Token op = previous();
                expr = new Expr.Binary(expr, op, unary());
            }
            return expr;
        }

        private Expr unary()
        {
            if(match(Not, Minus)){
                Token op = previous();
                return new Expr.Unary(op, unary());
            }
            return primary();
        }

        private Expr primary()
        {
            if(match(True)){return new Expr.Literal(true);}
            if(match(False)){return new Expr.Literal(false);}

            if(match(NUM)){
                return new Expr.Literal(int.Parse(previous().value));
            }

            if(match(TokenType.String)){
                return new Expr.Literal(previous().value);
            }

            if(match(ParSta)){
                Expr e = expr();
                expect("Expect ')' after expression!", ParEnd);
                return new Expr.Grouping(e);
            }

            throw error("Expect expression!");
        }

        #endregion

        bool newVar(){
            if(match(TokenType.ID)){
                if(match(TokenType.Colon)){
                    if(match(TokenType.ID)){
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}