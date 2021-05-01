using System.Diagnostics;

namespace trilc
{
    class Var
    {
        TrilType type;
        string name;

        public Var(TrilType type, string name)
        {
            this.type = type;
            this.name = name;
        }
    }
}