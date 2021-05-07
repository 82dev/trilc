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

        public Stmt.Expr equality(){
            Stmt.Expr e = comparision();
            if(match(TokenType.Equal)||match(TokenType.NotEqual)){
                Token op = previous();
                Stmt.Expr right = expr();
                e = new Stmt.Expr.Binary(e, op, right);
            }
            return e;
        }

        public Stmt.Expr comparision(){
            Stmt.Expr e = term();
            if(match(TokenType.Greater)||match(TokenType.GreaterEq)
               ||match(TokenType.Lesser)||match(TokenType.LesserEq)){
                Token op = previous();
                Stmt.Expr right = term();
                e = new Stmt.Expr.Binary(e, op, right);
            }
            return e;
        }

        public Stmt.Expr term(){
            Stmt.Expr e = factor();
            if(match(TokenType.Plus)||match(TokenType.Minus)){
                Token op = previous();
                Stmt.Expr right = factor();
                e = new Stmt.Expr.Binary(e, op, right);
            }
            return e;
        }

        public Stmt.Expr factor(){
            Stmt.Expr e = unary();
            if(match(TokenType.Asterisk)||match(TokenType.Slash)){
                Token op = previous();
                Stmt.Expr right = unary();
                e = new Stmt.Expr.Binary(e, op, right);
            }
            return e;
        }

        public Stmt.Expr unary(){
            if(match(TokenType.Not) || match(TokenType.Minus)){
                Token op = previous();
                Stmt.Expr e = unary();
                return new Stmt.Expr.Unary(op, e);
            }

            return literal();
        }

        public Stmt.Expr literal(){
            Token now = cur;
            if(match(TokenType.NUM)){
                return new Stmt.Expr.Literal(int.Parse(now.value));
            }
            if(match(TokenType.ParSta)){
                Stmt.Expr e = expr() as Stmt.Expr;
                expect("Expect ')' after expression", TokenType.ParEnd);
                return new Stmt.Expr.Grouping(e);
            }
            return null;
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