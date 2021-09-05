#include "includes.h"
#include "Lexer.h"
#include "error_codes.h"
#include "libtermcolor\termcolor.h"
#include <time.h>

int EndsWith(char *string, char *end);
static char *readFile(const char *path);

int main(int argc, char const *argv[])
{
    clock_t begin = clock();

    if(argc == 2){
        if(EndsWith(argv[1], ".tril")){
            initLexer(readFile(argv[1]));

            Token* token;
            while(true){
                token = getNextToken();
                
                if(token == NULL){
                    printf("Token was NULL!");
                    break;
                }

                if(token->type == EOF_TT){
                    break;
                }
                printf("%d %.*s\n", token->type, token->length, token->start);
                free(token);
            }

            clock_t end = clock();
            tcol_printf("{G}%f", (double)(end-begin)/CLOCKS_PER_SEC);
            // getchar();
            exit(0);
        }
        else{
            tcol_printf("{+R}Invalid argument: %s. File should have .tril extension\n", argv[1]);
            tcol_printf("Error : %d\n", INV_ARGS);
            exit(INV_ARGS);
        }
    }
    else{
        tcol_printf("{+R}Usage: trilc [path]\n");
        tcol_printf("Error : %d\n", NO_ARGS);
        exit(NO_ARGS);
    }
}

//https://stackoverflow.com/questions/744766/how-to-compare-ends-of-strings-in-c
int EndsWith(char *string, char *end)
{
    string = strrchr(string, end[0]);

    if(string != NULL ){
        return(strcmp(string, end) == 0);
    }

    return(0);
}

static char *readFile(const char *path){
    FILE *fp = fopen(path, "rb");

    if(fp == NULL){
        tcol_printf("{+R}Could not open file: %s\n", path);
        tcol_printf("Error : %d\n", INV_FILE);
        exit(INV_FILE);
    }

    fseek(fp, 0L, SEEK_END);
    unsigned int fileSize = ftell(fp);
    rewind(fp);

    char *buff = (char *)malloc(fileSize+1);
    unsigned int bytesRead = fread(buff, sizeof(char), fileSize, fp);
    buff[bytesRead] = '\0';

    fclose(fp);
    return buff;
}