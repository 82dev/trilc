use std::fmt::Debug;

#[derive(Debug)]
#[derive(PartialEq)]
pub enum TokenKind{
  Identifier(String),

  Number(i32),

  BraceOpen,
  BraceClose,

  ParenOpen,
  ParenClose,

  Semicolon,

  EOF,

  Unexpected,
}

pub struct Token{
  pub kind: TokenKind,
  pub line: usize,
  pub col: usize,
}

impl Token{
  pub fn new(kind: TokenKind, line: usize, col: usize) -> Token{
    Token{
      kind,
      line,
      col,
    }
  }
}

impl Debug for Token{
  fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
    write!(f, "Token: [Kind: {:?}, Line: {}, Column: {}]\n", self.kind, self.line, self.col)
  }
}