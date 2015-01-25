using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicGraphSearch
{
    class Program
    {
        public class GraphSearchAlgorithm
        {
            public Graph BuildFriendGraph()
            {
                Graph Aaron = new Graph("Aaron");
                Graph Betty = new Graph("Betty");
                Graph Brian = new Graph("Brian");
                Aaron.IsFriendOf(Betty);
                Aaron.IsFriendOf(Brian);

                Graph Catherine = new Graph("Catherine");
                Graph Carson = new Graph("Carson");
                Graph Darian = new Graph("Darian");
                Graph Derek = new Graph("Derek");
                Betty.IsFriendOf(Catherine);
                Betty.IsFriendOf(Darian);
                Brian.IsFriendOf(Carson);
                Brian.IsFriendOf(Derek);

                return Aaron;
            }

            /*
             * Поиск в глубину, кажда ветка просматривается конца
             * Алгоритм: В цикле рекурсивно просматриваем узлы, по порядку
             */
            public Graph DFS(Graph root, string nameToSearchFor)
            {
                if (nameToSearchFor == root.Name)
                    return root;

                Graph personFound = null;
                for (int i = 0; i < root.Friends.Count; i++)
                {
                    personFound = DFS(root.Friends[i], nameToSearchFor);
                    if (personFound != null)
                        break;
                }
                return personFound;
            }

            /*
             * Поиск в ширину, ишется значение на каждом уровне графа
             * Алгоритм: Добавляем узел графа в очередь, и в Сет
             * В цикле добавляем следующий уровень узлов, если их еще нет в сете,
             * и потом снова берем из очереди для сравнения
             */
            public Graph BFS(Graph root, string nameToSearchFor)
            {
                //FIFO - первым вошел, первым вышел
                Queue<Graph> Q = new Queue<Graph>();
                HashSet<Graph> S = new HashSet<Graph>();

                Q.Enqueue(root);
                S.Add(root);

                while (Q.Any())
                {
                    Graph p = Q.Dequeue();
                    if (p.Name == nameToSearchFor)
                        return p;
                    foreach (Graph friend in p.Friends)
                    {
                        if (!S.Contains(friend))
                        {
                            Q.Enqueue(friend);
                            S.Add(friend);
                        }
                    }
                }
                return null;
            }

            public void PrintGraph(Graph root)
            {
                Console.WriteLine(root.Name);
                for (int i = 0; i < root.Friends.Count; i++)
                {
                    PrintGraph(root.Friends[i]);
                }
            }
        }

        static void Main(string[] args)
        {
            GraphSearchAlgorithm b = new GraphSearchAlgorithm();
            Graph root = b.BuildFriendGraph();
            Console.WriteLine("Graph\n------");
            b.PrintGraph(root);

            Console.WriteLine("\nDepth-First Search\n------");
            Graph p = b.DFS(root, "Catherine");
            Console.WriteLine(p == null ? "Person not found" : p.Name);
            p = b.DFS(root, "Alex");
            Console.WriteLine(p == null ? "Person not found" : p.Name);

            Console.WriteLine("\nBreadth-First Search\n------");
            p = b.BFS(root, "Catherine");
            Console.WriteLine(p == null ? "Person not found" : p.Name);
            p = b.BFS(root, "Alex");
            Console.WriteLine(p == null ? "Person not found" : p.Name);
            Console.ReadLine();
        }
    }
}
