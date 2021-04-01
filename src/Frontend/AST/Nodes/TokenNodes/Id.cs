namespace trilc
{
    class IdTkNode : TokenNode
    {
        public IdTkNode(string v):base(v){}

        public override bool same(Node oth){
            return (this.value == string.Empty)?(this.GetType() == oth.GetType()):base.same(oth);
        }
    }
}