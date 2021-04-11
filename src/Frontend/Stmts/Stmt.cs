using System.Collections.Generic;

namespace trilc
{
    public abstract class Stmt
    {
        List<Stmt> children;
        public class Block : Stmt
        {
            public Block(List<Stmt> t){
                children = t;
            }   
        }

        public class Dec : Stmt{
            string name;
            string type;
            public Dec(string n, string t){
                (name, type) = (n,t);
            }
        }

        public class newAssign : Stmt{
            string name;
            Expr value;
            Token type;
            public newAssign(string n, Expr v, Token t)
            {
                (name,value, type) = (n,v,t);
            }
        }

        public class Empty : Stmt{}

        public abstract class Expr:Stmt{

            public class Grouping : Expr{
                Expr expr;

                public Grouping(Expr e)
                {
                    this.expr = e;
                }
            }

            public class Binary : Expr{
                Expr left;
                Token op;
                Expr right;

                public Binary(Expr l, Token o, Expr r){
                    left = l;
                    op = o;
                    right = r;
                }
            }

            public class Unary : Expr{
                Token op;
                Expr expr;

                public Unary(Token o, Expr e){
                    op = o;
                    expr = e;
                }
            }

            public class Literal : Expr{
                object value;

                public Literal(object value)
                {
                    this.value = value;
                }
            }
        }
    }
}