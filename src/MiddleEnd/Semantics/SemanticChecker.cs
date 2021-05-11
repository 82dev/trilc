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
            environment.define(varStmt.name, varStmt.type.ToString(), varStmt.value);
        }
    }
}