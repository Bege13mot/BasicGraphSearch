using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicGraphSearch
{
    public class Graph
    {
        public Graph(string name)
            {
                this.Name = name;
            }

            public string Name { get; set; }

            List<Graph> _friendsList = new List<Graph>();

            public List<Graph> Friends
            {
                get
                {
                    return _friendsList;
                }
            }

            public void IsFriendOf(Graph p)
            {
                _friendsList.Add(p);
            }

            public override string ToString()
            {
                return Name;
            }
        
    }
}
