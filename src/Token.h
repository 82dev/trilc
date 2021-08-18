#ifndef TRIL_TOKEN
#define TRIL_TOKEN

#include "includes.h"

typedef enum{
    EOF_TT,
    ID_TT
} TokenType;

typedef struct
{
    int col, line;
    char *value;
    TokenType type;
    char *file;
}Token;

Token* createToken(int col, int line, char *value, TokenType type, char *file);

#endif