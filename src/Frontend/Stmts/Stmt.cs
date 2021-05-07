using System.Collections.Generic;

namespace trilc
{
    public abstract class Stmt
    {
        public class Program
        {
            public List<Stmt> children;

            public Program(List<Stmt> t){
                children = t;
            }
        }

        public class Block : Stmt
        {
            public List<Stmt> children;
            public Block(List<Stmt> t){
                children = t;
            }   
        }

        public class Dec : Stmt{
            string name;
            Token type;
            public Dec(string n, Token t){
                (name, type) = (n,t);
            }
        }

        public class newAssign : Stmt{
            public string name;
            public Stmt.Expr value;
            public Token type;
            public newAssign(string n, Stmt.Expr v, Token t)
            {
                (name,value, type) = (n,v,t);
            }
        }

        public class Empty : Stmt{}

        public abstract class Expr:Stmt{

            public class Grouping : Expr{
                public Expr expr;

                public Grouping(Expr e)
                {
                    this.expr = e;
                }
            }

            public class Binary : Expr{
                public Expr left;
                public Token op;
                public Expr right;

                public Binary(Expr l, Token o, Expr r){
                    left = l;
                    op = o;
                    right = r;
                }
            }

            public class Unary : Expr{
                public Token op;
                public Expr expr;

                public Unary(Token o, Expr e){
                    op = o;
                    expr = e;
                }
            }

            public class Literal : Expr{
                public object value;

                public Literal(object value)
                {
                    this.value = value;
                }
            }
        }
    }
}