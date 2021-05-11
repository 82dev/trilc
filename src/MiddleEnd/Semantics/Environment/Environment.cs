using System.Collections.Generic;
using System.Linq;

namespace trilc
{
    class Environment
    {
        Dictionary<(string name, string type), Stmt.Expr> values = new Dictionary<(string name, string type), Stmt.Expr>();
        Environment enclosing = null;

        public Environment(){}

        public Environment(Environment envr){
            enclosing = envr;
        }

        public void define(string n, string t, Stmt.Expr e)
        {
            if(!has(n, t))
            {
                values.Add((n, t), e);
                return;
            }
            throw error($"Variable '{n}' already exists!");
        }

        public void define(string n, string t)
        {
            if(!has(n, t))
            {
                values.Add((n, t), null);
            }
            throw error($"Variable '{n}' already exists!");
        }

        public Stmt.Expr get(string n, string t)
        {
            if(has(n, t))
            {
                var e = values[(n, t)];
                if(e != null)
                {
                    return e;
                }
                throw error($"Uninitialized variable '{n}'!");
            }
            throw error($"Variable '{n}' not found");     
        }

        public SemanticException error(string msg){
            Error.assert(msg);
            return new SemanticException();
        }

        public bool has(string n, string t)
        {
            foreach (var item in values.Keys)
            {
                if((item.name == n )&&(item.type == t)){
                    return true;
                }
            }   
            return false;
        }       
    }
}