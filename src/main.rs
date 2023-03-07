mod token;
mod lexer;

fn main(){
  let test_string = String::from("; \n ;;");
  println!("{:?}", lexer::tokenize(test_string.chars().collect()));
}