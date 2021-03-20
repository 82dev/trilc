namespace trilc
{
    class Expression:Node
    {
        bool single;
        public string value;
        Operation operation;
        Expression lNode;
        Expression rNode;

        public Expression(Expression l, Expression r, Operation o)
        {
            lNode = l;
            rNode = r;
            operation = o;
            calcValue();
        }

        private Expression(string v){
            value = v;
            single = true;
        }

        public Expression(int l, int r, Operation o)
        {
            lNode = new Expression(l.ToString());
            rNode = new Expression(r.ToString());
            operation = o;
            calcValue();
        }

        public Expression calcValue(){
            if(single != true){
                value = operation.calcValue(int.Parse(lNode.value), int.Parse(rNode.value));
            }
            return this;
        }
    }
}