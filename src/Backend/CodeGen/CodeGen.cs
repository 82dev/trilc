using System.Collections.Generic;
using static trilc.InstructionType;

namespace trilc
{
    class CodeGen
    {
        Dictionary<string, int> variables = new Dictionary<string, int>();
        int lastVarI = 0;
        List<Instruction> instructions = new List<Instruction>();

        void emit(InstructionType it, params int[] pa){
            instructions.Add(new Instruction(it, pa));
        }

        public Instruction[] genProg(Stmt.Program program){
            generate(program.children);
            emit(InstructionType.halt);
            return instructions.ToArray();
        }

        void generate(List<Stmt> stmts){
            foreach (var stmt in stmts)
            {
                if(stmt is Stmt.If ifStmt){genIf(ifStmt);}
                if(stmt is Stmt.Block block){genBlock(block);}
                if(stmt is Stmt.Var varStmt){genVar(varStmt);}
                if(stmt is Stmt.ReAss reAss){genReAss(reAss);}
            }
        }

        void genIf(Stmt.If ifStmt){
            genExpr(ifStmt.expr);
            emit(InstructionType.jfal, 0);
            int b4 = instructions.Count;
            genBlock(ifStmt.body);
            
            instructions[b4 - 1] = new Instruction(InstructionType.jfal, 
                (b4 + (instructions.Count - b4) + 1)
            );
        }
        void genReAss(Stmt.ReAss reAss){
            genExpr(reAss.value);
            emit(InstructionType.load, variables[reAss.name]);
        }
        void genBlock(Stmt.Block block){
            int i = lastVarI;
            var vars = new Dictionary<string, int>(variables);
            generate(block.children);
            variables = new Dictionary<string, int>(vars);
            lastVarI = i;
        }
        void genVar(Stmt.Var varStmt){
            genExpr(varStmt.value);
            emit(InstructionType.store, lastVarI);
            variables.Add(varStmt.name, lastVarI);
            lastVarI++;
        }
        
        void genExpr(Stmt.Expr expr){
            if(expr is Stmt.Expr.Binary bin){
                Dictionary<TokenType, InstructionType> opDIct = new Dictionary<TokenType, InstructionType>(){
                    {TokenType.Plus, InstructionType.add},
                    {TokenType.Minus, InstructionType.sub},
                    {TokenType.Asterisk, InstructionType.mul},
                    {TokenType.Slash, InstructionType.div},

                    {TokenType.Equal, InstructionType.eq},
                    {TokenType.NotEqual, InstructionType.neq},
                    {TokenType.Greater, InstructionType.gr},
                    {TokenType.GreaterEq, InstructionType.greq},
                    {TokenType.Lesser, InstructionType.ls},
                    {TokenType.LesserEq, InstructionType.lseq},
                };

                genExpr(bin.right);
                genExpr(bin.left);
                emit(opDIct[bin.op.tokenType]);
            }

            if(expr is Stmt.Expr.Literal<int>.intLiteral intL){emit(InstructionType.push, intL.value);}
            if(expr is Stmt.Expr.Literal<bool>.boolLiteral boolL){
                int boolV = boolL.value == true ?1:0;
                emit(InstructionType.push, boolV);
            }
            if(expr is Stmt.Expr.Literal<string>.varLiteral varL){emit(InstructionType.load, variables[varL.value]);}
            if(expr is Stmt.Expr.Grouping gro){genExpr(gro.expr);}
        }
    }
}