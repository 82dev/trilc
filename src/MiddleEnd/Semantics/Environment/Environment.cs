
using System.Collections.Generic;
using System.Linq;

namespace trilc
{
    class Environment
    {
        Dictionary<(string name, string type), Stmt.Expr> values = new Dictionary<(string name, string type), Stmt.Expr>();
        public Environment enclosing = null;

        public Environment(){}

        public Environment(Environment envr){
            enclosing = envr;
        }

        public void define(string n, string t, Stmt.Expr e)
        {
            if(!has(n))
            {
                values.Add((n, t), e);
                return;
            }
            throw error($"Variable '{n}' already exists!");
        }

        public void reDefine((string name, string type) key, Stmt.Expr e){
            if(enclosing != null && enclosing.has(key.name)){
                enclosing.reDefine(key, e);
            }
            values[key] = e;
        }

        public Stmt.Expr get(string n)
        {
            // if(has(n))
            // {
            //     var e = values[(n, t)];
            //     if(e != null)
            //     {
            //         return e;
            //     }
            //     throw error($"Uninitialized variable '{n}'!");
            // }
            foreach (var item in values.Keys)
            {
                if(item.name == n){
                    var e = values[item];
                    if(e != null)
                    {
                        return e;
                    }
                    throw error($"Uninitialized variable '{n}'!");
                }
            }

            if(enclosing != null){
                return enclosing.get(n);
            }

            throw error($"Variable '{n}' not found");     
        }

        public (string name, string type) getKey(string n){
            for (int i = 0; i < values.Keys.Count; i++)
            {
                if(values.Keys.ToArray()[i].name == n){
                    return values.Keys.ToArray()[i];
                }
            }
            if(enclosing != null){
                return enclosing.getKey(n);
            }
            throw error($"Variable '{n}' doesn't exist!");
        }

        public SemanticException error(string msg){
            Error.assert(msg);
            return new SemanticException();
        }

        public bool has(string n)
        {
            foreach (var item in values.Keys)
            {
                if((item.name == n )/*&&(item.type == t)*/){
                    return true;
                }
            }   
            return false;
        }       
    }
}