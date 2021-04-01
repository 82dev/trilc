namespace trilc
{
    public static class ext
    {
        public static bool same(this Node[] src, Node[] oth)
        {
            if(src.Length != oth.Length){
                return false;
            }
            int i = 0;
            while (i < src.Length)
            {
                if(!src[i].same(oth[i])){
                    return false;
                }
                i++;
            }
            return true;
        }

        public static bool sameType(this Node[] src, Node[] oth)
        {
            if(src.Length != oth.Length){
                return false;
            }
            int i = 0;
            while (i < src.Length)
            {
                if(!src[i].sameType(oth[i])){
                    return false;
                }
                i++;
            }
            return true;
        }
    }
}