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

        Comma,
        SemiColon,
        Colon,
        Plus,Increase, PlusEqual,
        Minus,Decrease, MinusEqual,
        Asterisk, MulEqual,
        Slash, SlashEqual,
        //=
        Assignment,
        
        
        //Pairs
        BlockStart, BlockEnd,
        ArrSta, ArrEnd,
        ParSta, ParEnd,

        //Booleans
        Equal, NotEqual, Not, TRUE, FALSE, Greater, GreaterEq, Lesser, LesserEq, And, Or,

        //Keywords
        USE, VOID, INT, BOOL, FN, ENSURE, IF, ELSE, WHILE,
    }
}