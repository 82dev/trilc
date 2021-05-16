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
                if(a != null)
                    stmts.Add(a);
            }
            return new Stmt.Program(stmts);
        }

        Stmt.Block block(){
            List<Stmt> stmts = new List<Stmt>();
            while (!match(TokenType.BlockEnd))
            {
                var a = stmt();
                if(a != null){
                    stmts.Add(a);
                }
                if(isEOF()){
                    throw error("Expect '}'!");
                }

            }
            return new Stmt.Block(stmts);
        }

        Stmt stmt(){
            try
            {
                if(match(ID)){
                    if(match(Colon) && (match(INT, BOOL))){
                        return Variable();
                    }
                    if(match(Assignment)){
                        return ReAss();
                    }
                }
                if(match(TokenType.BlockStart)){
                    return block();
                }
                if(match(TokenType.SemiColon)){
                    return new Stmt.Empty();
                }
                if(match(TokenType.IF)){
                    return ifStmt();
                }
                if(match(TokenType.WHILE)){return whileLoop();}
                
                throw error($"Unrecognized token '{cur.tokenType}'!");
            }
            catch (ParseException)
            {
                synchronize();
            }
            return null;
        }

        Stmt.While whileLoop(){
            expect("'(' after 'while'!", TokenType.ParSta);
            Expr e = expr();
            expect("')' after expression!", TokenType.ParEnd);
            expect("'{' after expression!", TokenType.BlockStart);
            Block b = block();
            return new While(e, b);
        }

        Stmt.Var Variable(){
            string n = peek(-3).value;
            Token t = previous();
            Expr e = null;
            if(match(Assignment)){
                e = expr();
            }
            expect("Expect ';'!", TokenType.SemiColon);
            return new Stmt.Var(n,e,t);
        }
        Stmt.ReAss ReAss(){
            string name = peek(-2).value;
            var e = expr();
            expect("Expect ';'!", SemiColon);
            return new ReAss(name, e);
        }

        Stmt.If ifStmt(){
            expect("Expect '(' at the start of if!", TokenType.ParSta);
            var e = expr();
            expect("Expect ')' at the end of expression!", TokenType.ParEnd);
            expect("Expect '{' after expression!", TokenType.BlockStart);
            var b = block();
            if(match(TokenType.ELSE)){
                expect("Expect '{' after else!", TokenType.BlockStart);
                var eb = block();
                return new Stmt.If(e, b, eb);
            }
            return new Stmt.If(e, b);
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
            if((expr is Expr.Binary eb)&&((eb.right is Expr.Literal<int> a)&&(a.value == 0))){
                throw error("Cannot divide by 0!");
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
            if(match(TRUE)){return new Expr.Literal<bool>.boolLiteral(true);}
            if(match(FALSE)){return new Expr.Literal<bool>.boolLiteral(false);}

            if(match(NUM)){
                return new Expr.Literal<int>.intLiteral(int.Parse(previous().value));
            }

            if(match(TokenType.String)){
                return new Expr.Literal<string>.stringLiteral(previous().value);
            }

            if(match(TokenType.ID)){
                return new Expr.Literal<string>.varLiteral(previous().value);
            }

            if(match(ParSta)){
                Expr e = expr();
                expect("Expect ')' after expression!", ParEnd);
                return new Expr.Grouping(e);
            }

            throw error("Expect expression!");
        }

        #endregion

        #endregion
    }
}