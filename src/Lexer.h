#ifndef TRILC_LEXER
#define TRILC_LEXER

#include "includes.h"
#include "Token.h"

typedef struct{
    const char *start;
    const char *current;
    int length;
    int line;
}Lexer;

void initLexer(const char *start);
Token *getNextToken();

#endif