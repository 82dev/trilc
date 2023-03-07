use crate::token::{Token, TokenType, self};

pub fn tokenize(input: Vec<char>) -> Vec<Token> {
  let mut tokens = vec![];

  let mut line = 1;
  for c in input{
    if c == '\n' {line+=1; continue;};
    if is_whitespace(c) {continue;}

    let token_type = match c {
      c @ 'a'..='Z' => TokenType::Identifier,
      ';' => TokenType::Semicolon,
      _ => TokenType::Unexpected,
    };


    tokens.push(Token::new(token_type, line));
  }

  tokens.push(Token::new(TokenType::EOF, line));

  tokens
}

fn is_whitespace(c: char) -> bool{
  match c{
    ' ' | '\n' | '\r' => true,
    _ => false
  }
}