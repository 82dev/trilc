namespace TrilComp
{
    public enum Operand{
        Add,
        Sub,
        Mul,
        Div,
    }

    class Expression:Node
    {
        bool single;
        public string value;
        Operand operand;
        Expression lNode;
        Expression rNode;

        public Expression(Expression l, Expression r, Operand o)
        {
            lNode = l;
            rNode = r;
            operand = o;
        }

        private Expression(string v){
            value = v;
            single = true;
        }

        public Expression(Expression l, Expression r)
        {
            lNode = l;
            rNode = r;
            calcValue();
        }

        public Expression(int l, int r)
        {
            lNode = new Expression(l.ToString());
            rNode = new Expression(r.ToString());
            calcValue();
        }

        public Expression calcValue(){
            value = (int.Parse(lNode.value) + int.Parse(rNode.value)).ToString();
            return this;
        }
    }
}