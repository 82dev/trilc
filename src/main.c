#include "includes.h"
#include "Token.h"
#include "libtermcolor\termcolor.h"

int main(int argc, char const *argv[])
{
    tcol_printf("{R}Hello {B}World{W}!");
    getchar();
    return 0;
}