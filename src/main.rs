use lexer::Lexer;

mod token;
mod lexer;
mod parser;

fn main(){
  let mut lexer = Lexer::new(include_str!("test.tril").chars().collect());
  println!("{:?}", lexer.tokenize());
}