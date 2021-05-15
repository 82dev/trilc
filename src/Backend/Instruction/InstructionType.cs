namespace trilc
{
    enum InstructionType
    {
        halt,

        //Direct stack manipulation
        push, pop,

        //bin ops
        add, sub, mul, div, 
        
        //boolean
        eq, neq, gr, greq, ls, lseq,

        //jumps
        jump, jtru, jfal,

        //Vars
        load, store,
    }
}