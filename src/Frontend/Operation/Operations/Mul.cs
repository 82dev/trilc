namespace trilc
{
    class Mul : Operation
    {
        public string calcValue(int l, int r)
        {
            return (l * r).ToString();
        }
    }
}