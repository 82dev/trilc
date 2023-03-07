#[derive(Debug)]
pub enum TokenType{
  Identifier,

  NumberLiteral,

  BraceOpen,
  BraceClose,

  ParenOpen,
  ParenClose,

  Semicolon,

  EOF,

  Unexpected,
}

#[derive(Debug)]
pub struct Token{
  token_type: TokenType,
  line: u32,
}

impl Token{
  pub fn new(token_type: TokenType, line: u32) -> Token{
    Token{
      token_type,
      line,
    }
  }
}