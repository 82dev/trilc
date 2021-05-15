namespace trilc
{
    class Instruction
    {
        public InstructionType type;
        public int[] @params;

        public Instruction(InstructionType t, params int[] p)
        {
            this.type = t;
            this.@params = p;
        }

    }
}