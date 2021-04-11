using System;
using System.Collections.Generic;

namespace trilc
{
    class Parser
    {
        bool error;
        Token[] tokens;
        Token cur{get => tokens[index];set => cur = value;}
        private int index = 0;

        #region check
        private bool same(TokenType tt){return cur.tokenType == tt;}
        private bool same(TokenType t, string value){if(same(t) && same(value)){return true;}return false;}
        private bool same(string v){if(cur.value == v){return true;}return false;}

        private bool match(TokenType tt){if(same(tt)){consume();return true;}return false;}
        private bool match(string v){if(same(v)){consume();return true;}return false;}
        private bool match(TokenType tt,string v){if(same(tt,v)){consume();return true;}return false;}

        private bool match(params TokenType[] tt){
            foreach (var item in tt)
            {
                if(same(item) == false){
                    return false;
                }
                consume();
            }
            return true;
        }

        private bool expect(string message, params TokenType[] tt){
            bool a = match(tt);
            if(!a)
            {
                error = true;
                new Error($"Error at ({cur.lineIndex},{cur.charIndex})! "+message);
                consume();
                throw new ParseException();
            }
            return a;
        }
        #endregion
        #region gets
        private Token previous(int i){return tokens[index-i];}
        private Token previous(){return tokens[index-1];}

        private void consume(){index++;}
        #endregion

        private void synchronize(){
            consume();
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

                consume();
            }
        }

        public Parser(string[] input)
        {
            tokens = new Lexer().lex(input);
        }

        public void parse(){
            var a = block();
        }

        public Stmt block(){
            List<Stmt> stmts = new List<Stmt>();
            expect("Expect \'{\'!",TokenType.BlockStart);
            do{
                var a = stmt();
                if(a != null){
                    stmts.Add(a);
                }
            }while(!match(TokenType.BlockEnd));//nice

            expect("Expect EOF", TokenType.EOF);
            return new Stmt.Block(stmts);
        }

        public Stmt stmt(){
            try
            {
                if(same(TokenType.BlockStart)){
                    return block();
                }
                if(match(TokenType.SemiColon)){
                    return new Stmt.Empty();
                }
                if(newVar()){
                    if(match(TokenType.Assignment)){
                        return newAss();
                    }
                    return dec();
                }
            }
            catch (ParseException error)
            {
                synchronize();
                return null;
            }
                
            return null;
        }

        public Stmt dec(){
            string name = previous(3).value;
            string type = previous().value;
            expect("Expect ';' ", TokenType.SemiColon);
            return new Stmt.Dec(name, type);
        }

        public Stmt newAss(){
            string name = previous(4).value;
            Token type = previous(2);
            Stmt.Expr exp = expr() as Stmt.Expr;
            expect("Expected ';'", TokenType.SemiColon);
            if(exp != null){    
                return new Stmt.newAssign(name, exp, type);
            }

            return null;
        }

        #region Expressions
        public Stmt expr(){
            return term();
        }

        public Stmt term(){
            Stmt.Expr expr = factor();
            if(match(TokenType.Plus)||match(TokenType.Minus)){
                Token op = previous();
                Stmt.Expr right = factor();
                expr = new Stmt.Expr.Binary(expr, op, right);
            }
            return expr;
        }

        public Stmt.Expr factor(){
            Stmt.Expr expr = unary();
            if(match(TokenType.Asterisk)||match(TokenType.Slash)){
                Token op = previous();
                Stmt.Expr right = unary();
                expr = new Stmt.Expr.Binary(expr, op, right);
            }
            return expr;
        }

        public Stmt.Expr unary(){
            if(match(TokenType.Not) || match(TokenType.Minus)){
                Token op = previous();
                Stmt.Expr expr = unary();
                return new Stmt.Expr.Unary(op, expr);
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
        
        public bool newVar(){
            if(match(TokenType.ID)){
                if(match(TokenType.Colon)){
                    if(type()){
                        return true;
                    }
                }
            }
            return false;
        }

        private bool type(){
            return match(TokenType.INT)||match(TokenType.BOOL);
        }
    }
}