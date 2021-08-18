#ifndef TRIL_TOKEN
#define TRIL_TOKEN

#include "includes.h"

enum TokenType{
    EOF_TT,

    ID_TT
};

typedef struct Token
{
    int col, line;
    char *value;
    TokenType type;
    char *file;
};


#endif