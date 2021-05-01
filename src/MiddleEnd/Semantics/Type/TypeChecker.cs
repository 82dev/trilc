using System.Collections.Generic;
using System.Linq;

namespace trilc
{
    public class TypeChecker
    {
        public static void CheckBlock(Stmt.Block program)
        {
            foreach (var stmt in program.children)
            {
                try
                {
                    check(stmt);
                }
                catch (TypeException)
                {
                    continue;
                } 
            }
        }

        private static void check(Stmt stmt){
            if(stmt is Stmt.Block block){
                CheckBlock(block);
            }
            if(stmt is Stmt.newAssign newAssign){
                checkNewAssign(newAssign);
            }
        }

        private static void checkNewAssign(Stmt.newAssign newAssign){
            TrilType type = fromExpr(newAssign.value);
            if(!type.same(TrilType.TYPE.getChild(newAssign.type.value))){
                new Error($"Error ({newAssign.type.lineIndex}, {newAssign.type.charIndex})! cannot convert expression to type '{newAssign.type.value}'");
                throw new TypeException();
            }
        }

        private static TrilType fromExpr(Stmt.Expr expr){
            if(expr is Stmt.Expr.Binary bin){
                switch (bin.op.tokenType)
                {
                    case TokenType.Equal:
                    case TokenType.NotEqual: 
                        if(!TypeChecker.fromExpr(bin.left).same(TypeChecker.fromExpr(bin.right))){
                            new Error($"Error ({bin.op.lineIndex}, {bin.op.charIndex})! Both operands should be same");
                            throw new TypeException();
                        }
                        return TrilType.TYPE.getChild("bool");
                        break;
                    
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Asterisk:
                    case TokenType.Slash:
                        if(TypeChecker.fromExpr(bin.left).same(TrilType.TYPE.getChild("int"))
                        && TypeChecker.fromExpr(bin.right).same(TrilType.TYPE.getChild("int"))){
                            return TrilType.TYPE.getChild("int");
                        }

                        new Error($"Error ({bin.op.lineIndex}, {bin.op.charIndex})! Both operads should be int");
                        throw new TypeException();
                        break;

                    case TokenType.Greater:
                    case TokenType.GreaterEq:
                    case TokenType.Lesser:
                    case TokenType.LesserEq:
                        if(TypeChecker.fromExpr(bin.left).same(TrilType.TYPE.getChild("int"))
                        && TypeChecker.fromExpr(bin.right).same(TrilType.TYPE.getChild("int"))){
                            return TrilType.TYPE.getChild("bool");
                        }
                        new Error($"Error ({bin.op.lineIndex}, {bin.op.charIndex})! Both operads should be int");
                        throw new TypeException();
                        break;

                    default: break;
                }
            }

            if(expr is Stmt.Expr.Grouping group){
                return fromExpr(group.expr);
            }

            if(expr is Stmt.Expr.Literal lit){
                if(lit.value is int){
                    return TrilType.TYPE.getChild("int");
                }
                if(lit.value is bool){
                    return TrilType.TYPE.getChild("bool");
                }
            }
            return null;
        }
    }
}