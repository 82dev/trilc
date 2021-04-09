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
                if(match(item) == false){
                    return false;
                }
            }
            return true;
        }

        private bool expect(string message, params TokenType[] tt){
            bool a = match(tt);
            if(!a)
            {
                new Error(message);
            }
            return a;
        }
        #endregion

        #region gets
        private Token previous(int i){return tokens[index-i];}
        private Token previous(){return tokens[index-1];}

        private void consume(){index++;}
        #endregion

        public Parser(string[] input)
        {
            tokens = new Lexer().lex(input);
        }

        public void parse(){
            expect("",TokenType.SOF);
            var a = block();
        }

        public Stmt block(){
            List<Stmt> stmts = new List<Stmt>();
            expect("Expect \'{\'",TokenType.BlockStart);
            do{

            }while(!match());
            return new Stmt.Block(stmts);
        }

        public Stmt stmt(){
            expect("Expect ';'", TokenType.SemiColon);
            return new Stmt.testDec();
        }
    }
}