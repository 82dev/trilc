
namespace trilc
{
    class TrilType
    {
        public static TrilType @object = new @object();
        public static TrilType @int = new @int();
        public static TrilType @bool = new @bool();

        public TrilType parent = null;

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}