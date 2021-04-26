using System.Collections.Generic;
using System.Linq;

namespace trilc
{
    public class TrilType
    {
        public static TrilType TYPE = new TrilType("TYPE");
        public readonly List<TrilType> derivatives = new List<TrilType>();
        public string name;

        public TrilType(TrilType p, string v)
        {
            name = v;
            if(!hasChild(p, v)){
                p.derivatives.Add(this);
            }
        }

        public TrilType(string v){
            if(v == "TYPE" && TYPE != null){
                return;
            }
            name = v;
            if(TYPE == null){
                TYPE = this; 
                new TrilType("int");
                new TrilType("bool");
                return;
            }

            if(!hasChild(TYPE,v)){
                TYPE.derivatives.Add(this);
            }
        }

        public TrilType getChild(string v){
            foreach (var item in derivatives)
            {
                if(item.name == v){
                    return item;
                }
            }
            return null;
        }

        public bool hasChild(string v){
            return getChild(v) != null;
        }

        public bool hasChild(TrilType type, string v){
            return type.hasChild(v);
        }

        public bool same(TrilType type){
            if(type == null){
                return false;
            }

            if(derivatives.Count != type.derivatives.Count){
                return false;
            }

            for (int i = 0; i < type.derivatives.Count; i++)
            {
                if(!(type.derivatives.ElementAt(i).same(derivatives.ElementAt(i)))){
                    return false;
                }
            }

            return type.name == name;
        }
    }
}