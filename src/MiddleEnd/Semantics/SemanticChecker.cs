using System.Collections.Generic;

namespace trilc
{
    class SemanticChecker
    {
        Environment environment = new Environment();

        public void Check(List<Stmt> statements, ref bool error)
        {
            int i = 0;
            for (;i < statements.Count; i++)
            {
                try
                {
                    if(statements[i] is Stmt.Var varStmt){checkVar(varStmt);}
                    if(statements[i] is Stmt.Block blockStmt){checkBlock(blockStmt, ref error);}
                    if(statements[i] is Stmt.ReAss reAss){checkReAss(reAss);}
                    if(statements[i] is Stmt.If ifS){checkIf(ifS, ref error);}
                }
                catch (SemanticException)
                {
                    error = true;
                }
            }
        }

        void checkBlock(Stmt.Block block, ref bool error){
            environment = new Environment(environment);
            Check(block.children, ref error);
            environment = environment.enclosing;
        }

        void checkVar(Stmt.Var varStmt)
        {
            TrilType tokT = fromToken(varStmt.type);
            if(varStmt.value != null){
                TrilType exprType = fromExpr(varStmt.value);
                if(exprType != tokT){
                    throw error($"Cannot convert type '{exprType.ToString()}' to '{tokT.ToString()}'");
                }
            }
            environment.define(varStmt.name, tokT, varStmt.value);
        }
        void checkReAss(Stmt.ReAss reAss){
            var key = environment.getKey(reAss.name);
            TrilType type = key.type;
            TrilType exprT = fromExpr(reAss.value);
            if(type != exprT){
                throw error($"Cannot convert '{exprT}' to '{type}'");
            }
            environment.reDefine(key, reAss.value);
        }
        void checkIf(Stmt.If ifStmt, ref bool err){
            TrilType exprT = fromExpr(ifStmt.expr);
            if(exprT != TrilType.@bool){
                throw error("Expression must be a boolean!");
            }
            checkBlock(ifStmt.body, ref err);
        }

        TrilType fromExpr(Stmt.Expr expr)
        {
            if(expr is Stmt.Expr.Binary bE){
                switch (bE.op.tokenType)
                {
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Asterisk:
                    case TokenType.Slash:{
                        if((fromExpr(bE.right) == TrilType.@int) &&
                           (fromExpr(bE.left) == TrilType.@int)){
                            return TrilType.@int;
                        }
                        throw error($"Operands of '{bE.op.value}' should be integers");
                    }

                    case TokenType.Greater:
                    case TokenType.GreaterEq:
                    case TokenType.Lesser:
                    case TokenType.LesserEq:{
                        if((fromExpr(bE.right) == TrilType.@int) &&
                           (fromExpr(bE.left) == TrilType.@int)){
                            return TrilType.@bool;
                        }
                        throw error($"Operands of '{bE.op.value}' should be integers");
                    }

                    case TokenType.Equal:
                    case TokenType.NotEqual:{
                        if(fromExpr(bE.right) == fromExpr(bE.left)){
                            return TrilType.@bool;
                        }
                        throw error($"Operands of '{bE.op.value}' should be the same");
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
            if(expr is Stmt.Expr.Literal<string>.varLiteral v){
                return fromExpr(environment.get(v.value));
            }

            throw error("Unrecognized expression!");
        }
        TrilType fromToken(Token tok){
            return tok.tokenType == TokenType.INT ? TrilType.@int : TrilType.@bool;
        }

        SemanticException error(string msg){
            Error.assert(msg);
            return new SemanticException();
        }
    }
}