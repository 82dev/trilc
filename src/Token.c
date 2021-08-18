#include "Token.h"

Token* createToken(int col, int line, char *value, TokenType type, char *file){
    Token *token = (Token *)malloc(sizeof(Token));
    token->col = col;
    token->line = line;
    token->value = value;
    token->type = type;
    token->file = file;
    return token;
}