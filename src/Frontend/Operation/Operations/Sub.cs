namespace trilc
{
    class Sub : Operation
    {
        public string calcValue(int l, int r)
        {
            return (l- r).ToString();
        }
    }
}