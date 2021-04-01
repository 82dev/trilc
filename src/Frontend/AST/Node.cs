using System.Collections.Generic;

namespace trilc
{
    public abstract class Node
    {
        public string value;
        public bool hasMid{get => mid == null;private set => hasMid = value;}
        public readonly List<List<Node>> children = new List<List<Node>>();
        Node mid;
        Node lNode;
        Node rNode;

        public Node(string v){
            this.value = v;
        }

        public virtual bool same(Node oth){
            return (this.GetType() == oth.GetType()) && this.value == oth.value;
        }
        public bool sameType(Node oth){
            return (this.GetType() == oth.GetType());
        }

        public Node getLNode(){
            return lNode;
        }
        public Node getRNode(){
            return rNode;
        }
        public Node getMid(){
            return mid;
        }

        public void setLNode(Node a){
            lNode = a;
        }
        public void setRNode(Node a){
            rNode = a;
        }
        public void setMid(Node a){
            mid = a;
        }

        public void AddChild(List<Node> l){
            children.Add(l);
        }
    }
}