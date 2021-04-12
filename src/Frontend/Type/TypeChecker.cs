namespace trilc
{
    class TypeChecker
    {
        public static string typeFromExpression(Stmt.Expr expr){
            if(expr is Stmt.Expr.Literal){
                Stmt.Expr.Literal exp = (Stmt.Expr.Literal)expr;
                if(exp.value is int)return "int";
            }
            return "";
        }
    }
}