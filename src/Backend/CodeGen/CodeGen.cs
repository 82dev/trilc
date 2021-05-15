using System.Collections.Generic;
using System.Linq;

namespace trilc
{
    class CodeGen
    {
        int instCount = 0;
        int lastVarI = 0;
        Dictionary<string, int> variables = new Dictionary<string, int>();

        public Instruction[] genProg(Stmt.Program prog){
            var a = generate(prog.children).ToList();
            a.Add(new Instruction(InstructionType.halt));
            return a.ToArray();
        }

        Instruction[] generate(List<Stmt> stmts){
            List<Instruction> instructions = new List<Instruction>();

            instCount = instructions.Count;
            foreach (var stmt in stmts)
            {
                instCount = instructions.Count;
                if(stmt is Stmt.Var varStmt){add(genVar(varStmt));}
                if(stmt is Stmt.ReAss reAss){add(genReAss(reAss));}
                if(stmt is Stmt.Block block){add(genBlock(block));}
                if(stmt is Stmt.If ifS){add(genIf(ifS));}
                instCount = instructions.Count;
            }

            void add(Instruction[] ins){
                foreach (var item in ins)
                {
                    instructions.Add(item);
                }
            }
            return instructions.ToArray();
        }

        Instruction[] genIf(Stmt.If ifStmt){
            List<Instruction> instructions = new List<Instruction>();
            void add(Instruction[] ins){
                foreach (var item in ins)
                {
                     instructions.Add(item);
                }
            }
            
            add(genExpr(ifStmt.expr));

            instructions.Add(new Instruction(InstructionType.jfal, 0));
            int b4 = instructions.Count;
            add(genBlock(ifStmt.body));
            instructions[b4 - 1] = new Instruction(InstructionType.jfal, (
                (b4 + (instCount - b4) + 1)
            ));

            // instructions.Add(new Instruction(InstructionType.jtru,(instCount + (instructions.Count + 3))));
            // instructions.Add(new Instruction(InstructionType.jump, 0));
            // int b4 = instructions.Count;
            // int jumpIndex = instructions.Count - 1;
            // add(genBlock(ifStmt.body));
            // instructions[jumpIndex] = new Instruction(InstructionType.jump, 
            //     instCount + ((instructions.Count - b4)+1)
            // );
            return instructions.ToArray();
        }
        Instruction[] genBlock(Stmt.Block block){
            var prevInstCount = instCount;
            int prevLast = lastVarI;
            var prevVars = new Dictionary<string, int>(variables);
            List<Instruction> instructions = new List<Instruction>();
            void add(Instruction[] ins){
                foreach (var item in ins)
                {
                    instructions.Add(item);
                }
            }
            
            add(generate(block.children));

            lastVarI = prevLast;
            instCount = prevInstCount;
            variables = new Dictionary<string, int>(prevVars);
            return instructions.ToArray();
        }
        Instruction[] genReAss(Stmt.ReAss reAss){
            List<Instruction> instructions = new List<Instruction>();
            void add(Instruction[] ins){
                 foreach (var item in ins)
                 {
                     instructions.Add(item);
                 }
            }
            
            add(genExpr(reAss.value));
            instructions.Add(new Instruction(InstructionType.store, variables[reAss.name]));
            return instructions.ToArray();
        }
        Instruction[] genVar(Stmt.Var varStmt){
            List<Instruction> instructions = new List<Instruction>();
            void add(Instruction[] ins){
                foreach (var item in ins)
                {
                    instructions.Add(item);
                }
            }
            add(genExpr(varStmt.value));
            instructions.Add(new Instruction(InstructionType.store, lastVarI));
            variables.Add(varStmt.name, lastVarI);
            lastVarI++;
            return instructions.ToArray();
        }
        Instruction[] genExpr(Stmt.Expr expr){
            List<Instruction> instructions = new List<Instruction>();
            void add(Instruction[] ins){
                foreach (var item in ins)
                {
                    instructions.Add(item);
                }
            }

            if(expr is Stmt.Expr.Binary bin){
                Dictionary<TokenType, InstructionType> opDIct = new Dictionary<TokenType, InstructionType>(){
                    {TokenType.Plus, InstructionType.add},
                    {TokenType.Minus, InstructionType.sub},
                    {TokenType.Asterisk, InstructionType.mul},
                    {TokenType.Slash, InstructionType.div},

                    {TokenType.Greater, InstructionType.gr},
                    {TokenType.GreaterEq, InstructionType.greq},
                    {TokenType.Lesser, InstructionType.ls},
                    {TokenType.LesserEq, InstructionType.lseq},
                };
                add(genExpr(bin.right));
                add(genExpr(bin.left));
                instructions.Add(new Instruction(opDIct[bin.op.tokenType]));
            }

            if(expr is Stmt.Expr.Literal<int>.intLiteral intL){
                instructions.Add(new Instruction(InstructionType.push, intL.value));
            }
            if(expr is Stmt.Expr.Literal<bool>.boolLiteral boolL){
                int boolV = boolL.value == true ?1:0;
                instructions.Add(new Instruction(InstructionType.push, boolV));
            }
            if(expr is Stmt.Expr.Literal<string>.varLiteral varL){
                instructions.Add(new Instruction(InstructionType.load, variables[varL.value]));
            }
            if(expr is Stmt.Expr.Grouping g){
                add(genExpr(g.expr));
            }
            
            return instructions.ToArray();
        }

        // Instruction[] gen(Stmt stmt){
        //     List<Instruction> instructions = new List<Instruction>();
        //     void add(Instruction[] ins){
        //         foreach (var item in ins)
        //         {
        //              instructions.Add(item);
        //         }
        //     }
        //     return instructions.ToArray();
        // }
    }
}