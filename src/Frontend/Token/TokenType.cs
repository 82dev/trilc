using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrilComp
{
    enum TokenType
    {
        EOF,
        CONST,
        BlockStart,
        BlockEnd,
        ID,
        NUM,
        Plus,
        Inc,
        Minus,
        Decrease,
        LineCom,
        ComStart,
        ComEnd,
        SemiColon,
        Dot,
        ArrSta,
        ArrEnd,
        ParSta,
        ParEnd,
    }
}