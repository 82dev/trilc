#include "Lexer.h"
#include "includes.h"

Token *eofToken();
Token *createToken(TokenType type);
static bool isAtEnd();
char *advance();
bool isalpha(char c);
bool isdigit(char c);
bool match(const char c);
void skipWhitspaces();
static inline const char *peekNext();
Token *identifier();
TokenType idType();
TokenType checkKeyword(int start, int length, 
                       const char* rest, TokenType type);

Lexer *lexer;

void initLexer(const char *start){
    lexer = (Lexer *)malloc(sizeof(Lexer));
    lexer->start = start;
    lexer->current = start;
    lexer->line = 1;
}

Token *getNextToken(){
    if(isAtEnd()){
        return eofToken();
    }

    skipWhitspaces();
    lexer->start = lexer->current;
    
    char *c = advance();

    if(isalpha(*c)){return identifier();}

    if(isdigit(*c)){
        while(isdigit(lexer->current[0])){
            advance();
        }
        return createToken(NUM_TT);
    }

    switch(*c)
    {
        case ';': return createToken(SEMICOLON_TT);
        case ':': return createToken(COLON_TT);

        case '=': 
            return createToken(match('=') ? ISEQ_TT : EQUAL_TT);
        
        case '+': return createToken(PLUS_TT);
        case '-': return createToken(MINUS_TT);
        case '*': return createToken(ASTERISK_TT);
        case '/': return createToken(SLASH_TT);

        case '!': 
            return createToken(match('=') ? ISNEQ_TT : NOT_TT);
        case '>':
            return createToken(match('=') ? GREQ_TT : GREAT_TT);
        case '<':
            return createToken(match('=') ? LSEQ_TT : LESS_TT);
        
        case '&':
            return createToken(match('&') ? AND_TT: BAND_TT);
        case '|':
            return createToken(match('|') ? OR_TT: BOR_TT);
        case '^': return createToken(BXOR_TT);

        case '(': return createToken(LPAREN_TT);
        case ')': return createToken(RPAREN_TT);
        case '{': return createToken(LBRACK_TT);
        case '}': return createToken(RBRACK_TT);

        default:
            break;
    }
    return NULL;
}

Token *identifier(){
    while(isalpha(*(lexer->current)) || isdigit(*(lexer->current))){
        advance();
    }
    return createToken(idType());
}

TokenType idType(){
    switch(lexer->start[0]){
        case 'i':{return checkKeyword(1, 1, "f", IF_TT);}
        case 'e':{return checkKeyword(1, 3, "lse", ELSE_TT);}
        case 'w':{return checkKeyword(1, 4, "hile", WHILE_TT);}
        case 'd':{return checkKeyword(1, 1, "o", DO_TT);}
    }

    return ID_TT;
}

TokenType checkKeyword(int start, int length, const char* rest, TokenType type){
    if(
        ((int)(lexer->current - lexer->start)) == start + length
        &&
        memcmp(lexer->start + start, rest, length) == 0
    ){
        return type;
    }
    return ID_TT;
}

void skipWhitspaces(){
    for(;;){
        char c = *(lexer->current);
        switch(c){
            case ' ':
            case '\t':
            case '\r':
                advance();
                break;
            
            case '\n':
                lexer->line++;
                advance();
                break;
            
            case '/':
                if(*peekNext() == '/'){
                    while(*(lexer->current) != '\n' && !isAtEnd()){advance();}
                }
                else{
                    return;
                }
                break;
            
            default:
                return;
        }
    }
}

static inline const char *peekNext(){
    if(isAtEnd()){return '\0';}
    return lexer->current + 1;
}

bool match(const char c){
    if(isAtEnd() 
      || 
      (*(lexer->current) != c)){
        return false;
    }
    lexer->current++;
    return true;
}

char *advance(){
    return lexer->current++;
}

static bool isAtEnd(){
    return *(lexer->current) == '\0';
}

bool isdigit(char c){
    return c >= '0' && c <= '9';
}

bool isalpha(char c){
    return (c >= 'a' && c <= 'z') ||
           (c >= 'A' && c <= 'Z') ||
            c == '_';
}

Token *createToken(TokenType type){
    Token *token = (Token *)malloc(sizeof(Token));
    token->start = lexer->start;
    token->length = (int)(lexer->current - lexer->start);
    token->line = lexer->line;
    token->type = type;
    return token;
}

Token *eofToken(){
    Token *token = (Token *)malloc(sizeof(Token));
    token->start = '\0';
    token->length = 0;
    token->line = 0;
    token->type = EOF_TT;
    return token;
}