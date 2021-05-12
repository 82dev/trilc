namespace trilc
{
    class SemanticChecker
    {
        Environment environment = new Environment();
        Stmt.Program program;

        public SemanticChecker(Stmt.Program prog)
        {
            program = prog;
        }

        public void Check()
        {
            var children = program.children;
            for (int i = 0; i < children.Count; i++)
            {
                if(children[i] is Stmt.Var v){
                    checkVar(v);
                }
            }
        }

        public void checkVar(Stmt.Var varStmt)
        {
            TrilType exprType = fromExpr(varStmt.value);
            TrilType tokT = fromToken(varStmt.type);

            if(exprType != tokT){
                throw error($"Cannot convert type '{exprType.ToString()}' to '{tokT.ToString()}'");
                return;
            }

            environment.define(varStmt.name, varStmt.type.ToString(), varStmt.value);
        }

        public TrilType fromExpr(Stmt.Expr expr)
        {
            if(expr is Stmt.Expr.Binary bE){
                switch (bE.op.tokenType)
                {
                    case TokenType.Plus:{
                        if((fromExpr(bE.right) == TrilType.@int) &&
                           (fromExpr(bE.left) == TrilType.@int)){
                            return TrilType.@int;
                        }
                        throw error("Operands of '+' should be integers");
                    }
                    case TokenType.Minus:{
                        if((fromExpr(bE.right) == TrilType.@int) &&
                           (fromExpr(bE.left) == TrilType.@int)){
                            return TrilType.@int;
                        }
                        throw error("Operands of '-' should be integers");
                    }
                    case TokenType.Asterisk:{
                        if((fromExpr(bE.right) == TrilType.@int) &&
                           (fromExpr(bE.left) == TrilType.@int)){
                            return TrilType.@int;
                        }
                        throw error("Operands of '*' should be integers");
                    }
                    case TokenType.Slash:{
                        if((fromExpr(bE.right) == TrilType.@int) &&
                           (fromExpr(bE.left) == TrilType.@int)){
                            return TrilType.@int;
                        }
                        throw error("Operands of '\\' should be integers");
                    }
                    default:
                        break;
                }
            }
            
            if(expr is Stmt.Expr.Grouping g){
                return fromExpr(g.expr);
            }

            if(expr is Stmt.Expr.Unary u){
                switch (u.op.tokenType)
                {
                    case TokenType.Minus: 
                        if(fromExpr(u.expr) == TrilType.@int){
                            return TrilType.@int;
                        }
                        throw error("Operand of '-' should be integer");
                    case TokenType.Not:
                        if(fromExpr(u.expr) == TrilType.@bool){
                            return TrilType.@bool;
                        }
                        throw error("Operand of '!' should be a booleans");;
                    default:throw error("");
                }
            }

            if(expr is Stmt.Expr.Literal<int>.intLiteral){
                return TrilType.@int;
            }
            if(expr is Stmt.Expr.Literal<bool>.boolLiteral){
                return TrilType.@bool;
            }
            throw error("");
        }

        public TrilType fromToken(Token tok){
            return tok.tokenType == TokenType.INT ? TrilType.@int : TrilType.@bool;
        }

        public SemanticException error(string msg){
            Error.assert(msg);
            return new SemanticException();
        }
    }
}