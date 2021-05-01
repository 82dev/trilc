namespace trilc
{
    class Instruction
    {
        public InstructionType type;
        public object[] @params;

        public Instruction(InstructionType t, object[] p)
        {
            type = t;
            @params = p;
        }

        public Instruction(InstructionType t){
            type = t;
        }
    }
}