namespace trilc
{
    class Div : Operation
    {
        public string calcValue(int l, int r)
        {
            return (l/r).ToString();
        }
    }
}