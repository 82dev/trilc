use std::{num, ops::Neg};

use crate::token::{Token, TokenKind, self};

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

        '/' => {
          if self.match_next('/'){
            while self.peek_next() != '\n'{
              self.advance();
            }
          }
          else if self.match_next('='){
            tokens.push(self.new_token(TokenKind::AssignmentDivide));
          }
          else {
            tokens.push(self.new_token(TokenKind::Slash));
          }
        },

        '-' => {
          if self.match_next('='){
            tokens.push(self.new_token(TokenKind::AssignmentSubtract));
          }
          else{
            tokens.push(self.new_token(TokenKind::Subtract));
          }
        }

        '+' => {
          if self.match_next('='){
            tokens.push(self.new_token(TokenKind::AssignmentAdd));
          }
          else{
            tokens.push(self.new_token(TokenKind::Plus));
          }
        }

        '*' => {
          if self.match_next('='){
            tokens.push(self.new_token(TokenKind::AssignmentMultiply));
          }
          else{
            tokens.push(self.new_token(TokenKind::Asterisk));
          }
        }

        '=' => {
          if self.match_next('='){
            tokens.push(self.new_token(TokenKind::CompEquality));
          }
          else {
            tokens.push(self.new_token(TokenKind::Assignment));
          }
        }

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
      self.advance();
      self.start = self.current;
    }

    tokens.push(self.new_token(TokenKind::EOF));

    tokens
  }

  fn identifier(&mut self) -> Token{
    while !self.is_at_end() && self.source[self.current].is_alphabetic(){
      self.advance();
    }

    let s = String::from_iter(self.source[self.start..self.current].iter());
    self.current -= 1;
    self.col -= 1;
    self.new_token(TokenKind::Identifier(s))
  }

  fn number(&mut self) -> Token{
    while !self.is_at_end() && self.source[self.current].is_ascii_digit() {
      //TODO: Im high in all ways but medical
      //if self.peek_next() != '\n' {self.advance();}
      if self.peek_next() == '\n'{
        break
      }
      self.advance()
    }
    self.new_token(
      TokenKind::Number(
        self.source[self.start..self.current]
          .iter()
          .collect::<String>()
          .parse()
          .expect("Couldnt parse string.")
      )
    )
  }

  fn match_next(&mut self, c: char) -> bool{
    if self.peek_next() == c{
      self.advance();
      return true 
    }
    false
  }

  fn peek_next(&mut self) -> char{
    if self.current == self.source.len() - 1{
      return self.source[self.current]
    }else{
      self.source[self.current + 1]
    }
  }

  fn advance(&mut self){
    if !self.is_at_end(){
      self.col += 1;
      self.current += 1;
    }
  }

  fn new_token(&mut self, kind: TokenKind) -> Token{
    Token::new(kind, self.line, self.col)
  }

  fn is_at_end(&self) -> bool{
    self.current >= self.source.len() - 1
  }
}