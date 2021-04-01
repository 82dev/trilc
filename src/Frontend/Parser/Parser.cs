using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace trilc
{
    class Parser
    {
        /*aa
            this is stack based, so to represent A -> 'a'+'b',
            left goes to right, right goes to left
        */
        Dictionary<Node[], Node> dict = new Dictionary<Node[], Node>(){
            {new Node[]{new SemiCoTkNode(""), new KeywordTkNode("int"), new ColonTkNode(""),new IdTkNode("")}, new AssTkNode("")},
            {new Node[]{new AssTkNode(""), new AssTkNode("")}, new root("")}
        };

        Dictionary<TokenType, Type> typeDict = new Dictionary<TokenType, Type>(){
            {TokenType.Keyword, typeof(KeywordTkNode)},
            {TokenType.ID, typeof(IdTkNode)},
            {TokenType.SemiColon, typeof(SemiCoTkNode)},
            {TokenType.Colon, typeof(ColonTkNode)},
        };

        Stack<Node> stack = new Stack<Node>();
        public CST parse(Token[] input){
            for(int i = 0; i < input.Length; i++){
                if(typeDict.ContainsKey(input[i].tokenType)){
                    stack.Push((Node)Activator.CreateInstance(typeDict[input[i].tokenType], input[i].value));
                }
                reduce();
            }
            reduce();
            return new CST((root)stack.Pop());
        }

        public bool reduce(){
            bool hasReduced = default;
            //try reduce
            List<Node> buffer = new List<Node>();
            //loop throughh stack frop top
            for (int j = 0; j < stack.Count; j++)
            {
                //add item from stack to list
                buffer.Add(stack.ElementAt(j));

                //loop through the keys of dictionary
                foreach (var item in dict.Keys.ToArray())
                {
                    //if buffer and key are same,
                    if(item.same(buffer.ToArray())){
                        hasReduced = true;
                        List<Node> children = new List<Node>();
                        //pop every item needed for reduction
                        int k = j+1;
                        while (k > 0)
                        {
                            children.Add(stack.Pop());
                            k--;
                        }

                        Node node = (Node)Activator.CreateInstance(dict[item].GetType(), dict[item].value);
                        node.AddChild(children);

                        //push reduction
                        stack.Push(node);
                    }
                }
            }

            return hasReduced;
        }
    }
}