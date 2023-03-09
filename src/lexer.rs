use crate::token::{Token, TokenKind};

pub struct Lexer{
  source: Vec<char>,
  line: usize,
  col: usize,
  current: usize,
  start: usize,
}

impl Lexer{
  pub fn new(source: Vec<char>) -> Lexer{
    Lexer{
      source,
      line: 1,
      col: 1,
      current: 0,
      start: 0,
    }
  }
}

impl Lexer{
  pub fn tokenize(&mut self) -> Vec<Token> {
    let mut tokens = vec![];

    while !self.is_at_end() {
      match self.source[self.current] {
        '\n' => {
          self.line += 1;
          self.col = 0;
        },

        '{' => tokens.push(self.new_token(TokenKind::BraceOpen)),
        '}' => tokens.push(self.new_token(TokenKind::BraceClose)),

        '(' => tokens.push(self.new_token(TokenKind::ParenOpen)),
        ')' => tokens.push(self.new_token(TokenKind::ParenClose)),

        ';' => tokens.push(self.new_token(TokenKind::Semicolon)),

        c => {
          if !c.is_ascii_whitespace(){
            if c.is_alphabetic(){
              tokens.push(self.identifier());
            }
            //TODO: negative
            else if c.is_ascii_digit(){
              tokens.push(self.number());
            }
            else {
              tokens.push(self.new_token(TokenKind::Unexpected));
            }
          }
        }
      }
      self.col += 1;
      self.current += 1;
      self.start = self.current;
    }

    tokens.push(self.new_token(TokenKind::EOF));

    tokens
  }

  fn identifier(&mut self) -> Token{
    while !self.is_at_end() && self.source[self.current].is_alphabetic(){
      self.col += 1;
      self.current += 1;
    }

    let s = String::from_iter(self.source[self.start..self.current].iter());
    self.current -= 1;
    self.col -= 1;
    self.new_token(TokenKind::Identifier(s))
  }

  fn number(&mut self) -> Token{
    while !self.is_at_end() && self.source[self.current].is_ascii_digit(){
      self.col += 1;
      self.current += 1;
    }
    let s = String::from_iter(self.source[self.start..self.current].iter());
    if let Ok(i) = s.parse::<i32>(){
      return self.new_token(TokenKind::Number(i))
    }
    //TODO: Error management in lexer
    self.new_token(TokenKind::Unexpected)
  }

  fn new_token(&mut self, kind: TokenKind) -> Token{
    Token::new(kind, self.line, self.col)
  }

  fn is_at_end(&self) -> bool{
    self.current >= self.source.len()
  }
}