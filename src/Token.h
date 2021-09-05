#ifndef TRIL_TOKEN
#define TRIL_TOKEN

#include "includes.h"

typedef enum{
    EOF_TT = 0,

    ID_TT,
    NUM_TT,

    SEMICOLON_TT,
    COLON_TT,
    
    EQUAL_TT,

    PLUS_TT,
    MINUS_TT,
    ASTERISK_TT,
    SLASH_TT,

    ISEQ_TT,
    ISNEQ_TT,
    GREAT_TT,
    LESS_TT,
    GREQ_TT,
    LSEQ_TT,

    AND_TT,
    OR_TT,
    NOT_TT,

    BAND_TT,
    BOR_TT,
    BXOR_TT,

    LPAREN_TT,
    RPAREN_TT,
    LBRACK_TT,
    RBRACK_TT,

    IF_TT,
    ELSE_TT,
    WHILE_TT,
    DO_TT,
}TokenType;

typedef struct
{
    uint line, length;
    const char *start;
    TokenType type;
}Token;

#endif