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

        public class testDec : Stmt{
            
        }
    }
}