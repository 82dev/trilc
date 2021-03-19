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
        Keyword,
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
        Assignment,
        Equal,
        NotEqual,
        Not,
        SemiColon,
        Dot,
        ArrSta,
        ArrEnd,
        ParSta,
        ParEnd,
    }
}