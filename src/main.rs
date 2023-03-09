mod token;
mod lexer;

use lexer::*;

fn main(){
  //TODO: fuck trhis shirt
  let mut lexer = Lexer::new(include_str!("test.tril").chars().collect());
  println!("{:?}", lexer.tokenize());
}