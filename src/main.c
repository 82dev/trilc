#include "includes.h"
#include "Token.h"

int main(int argc, char const *argv[])
{
    Token *token = createToken(0, 0, "\0", EOF_TT, "hello");
    getchar();
    free(token);
    return 0;
}