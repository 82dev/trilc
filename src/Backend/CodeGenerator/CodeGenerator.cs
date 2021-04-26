// using System.Collections.Generic;

// namespace trilc
// {
//     class CodeG{
//         int varI = 0;

//         Dictionary<TokenType, InstructionType> opDict = new Dictionary<TokenType, InstructionType>(){
//             {TokenType.Plus,InstructionType.Add},
//             {TokenType.Minus, InstructionType.Sub},
//             {TokenType.Asterisk, InstructionType.Mul},
//             {TokenType.Slash, InstructionType.Div},
//         };

//         Dictionary<string, int> vars = new Dictionary<string, int>();

//         public List<Instruction> fromStmt(Stmt stmt){
//             List<Instruction> instructions = new List<Instruction>();

//             if(stmt is Stmt.newAssign na){
//                 add(fromExpr(na.value), ref instructions);
//                 instructions.Add(new Instruction(InstructionType.Store, new object[]{varI}));
//                 vars.Add(na.name, varI);
//                 varI++;
//             }

//             if(stmt is Stmt.Block bl){
//                 foreach (var item in bl.children)
//                 {
//                     add(fromStmt(item), ref instructions);
//                 }
//             }

//             return instructions;
//         }

//         public void writeBC(string path, List<Instruction> list){
//             Dictionary<InstructionType, string> dict = new Dictionary<InstructionType, string>(){
//                 {InstructionType.Push, "push"},
//                 {InstructionType.Store, "store"},
//                 {InstructionType.Sub, "sub"},
//                 {InstructionType.Mul, "mul"},
//                 {InstructionType.Add, "add"},
//                 {InstructionType.Div, "div"},
//             };

//             string BC = string.Empty;
//             foreach (var item in list)
//             {
//                 if(dict.ContainsKey(item.type)){
//                     BC += dict[item.type];
//                 }

//                 if(item.@params != null){
//                     foreach (var p in item.@params)
//                     {
//                         BC += $" {p.ToString()}";
//                     }
//                 }

//                 BC += "\n";
//             }

//             System.IO.File.WriteAllText(path, BC);
//         }

//         private List<Instruction> fromExpr(Stmt.Expr expr){
//             List<Instruction> instructions = new List<Instruction>();
//             if(expr is Stmt.Expr.Literal el){
//                 instructions.Add(new Instruction(InstructionType.Push, new object[]{el.value}));
//             }
//             if(expr is Stmt.Expr.Binary eb){
//                 var left = fromExpr(eb.left);
//                 var op = eb.op;
//                 var right = fromExpr(eb.right);
                
//                 add(left, ref instructions);
//                 add(right, ref instructions);

//                 instructions.Add(new Instruction(opDict[op.tokenType]));
//             }
//             if(expr is Stmt.Expr.Grouping eg)
//             {   
//                 add(fromExpr(eg.expr), ref instructions);
//             }

//             return instructions;
//         }

//         private void add(List<Instruction> list, ref List<Instruction> inst){
//             foreach (var item in list)
//             {
//                 inst.Add(item);
//             }
//         }
//     }
// }