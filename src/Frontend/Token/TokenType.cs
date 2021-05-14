using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trilc
{
    public enum TokenType
    {
        EOF,

        //Literal
        ID, NUM, String,

        Dot,
        SemiColon,
        Colon,
        Plus,Increase,
        Minus,Decrease,
        //=
        Assignment,
        Asterisk,
        Slash,
        
        //Pairs
        BlockStart, BlockEnd,
        ComStart, ComEnd,
        ArrSta, ArrEnd,
        ParSta, ParEnd,

        //Booleans
        Equal, NotEqual, Not, TRUE, FALSE, Greater, GreaterEq, Lesser, LesserEq,

        //Keywords
        USE, VOID, INT, BOOL, FUNC, ENSURE, IF, ELSE,
    }
}