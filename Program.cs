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

        
        /*
            * Алгоритм Дейкстры. Поиск кратчайшего пути во взвешенном графе. 
            * На основе https://github.com/mburst/dijkstras-algorithm
            * Алгоритм: Проставляем начальной вершине метку 0, для остальных max of int
            * Затем ищем вершину с минимальным растоянием и меткой 0. Затем добавляем в список, и удаляем из списка
            * те вершины, растояние (вес) у которых масимальный, если минимальный, то наоборот добавляем. 
            * Цикл остановится когда у всех вершин метка 0 или max int. Последний случай значит что граф не связанный.
            * Сложность - квадратичная.
            */

        class DijkstrasSearch
        {
            Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();

            public void AddVertex(char name, Dictionary<char, int> edges)
            {
                vertices[name] = edges;
            }

            public List<char> ShortestPath(char start, char finish)
            {
                var previous = new Dictionary<char, char>();
                var distances = new Dictionary<char, int>();
                var nodes = new List<char>();

                List<char> path = null;

                foreach (var vertex in vertices)
                {
                    if (vertex.Key == start)
                    {
                        distances[vertex.Key] = 0;
                    }
                    else
                    {
                        distances[vertex.Key] = int.MaxValue;
                    }

                    nodes.Add(vertex.Key);
                }

                while (nodes.Count != 0)
                {
                    nodes.Sort((x, y) => distances[x] - distances[y]);

                    var smallest = nodes[0];
                    nodes.Remove(smallest);

                    if (smallest == finish)
                    {
                        path = new List<char>();
                        while (previous.ContainsKey(smallest))
                        {
                            path.Add(smallest);
                            smallest = previous[smallest];
                        }

                        break;
                    }

                    if (distances[smallest] == int.MaxValue)
                    {
                        break;
                    }

                    foreach (var neighbor in vertices[smallest])
                    {
                        var alt = distances[smallest] + neighbor.Value;
                        if (alt < distances[neighbor.Key])
                        {
                            distances[neighbor.Key] = alt;
                            previous[neighbor.Key] = smallest;
                        }
                    }
                }

                return path;
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
            
            Console.WriteLine("\nDijkstra's algorithm\n------");
            DijkstrasSearch d = new DijkstrasSearch();
            d.AddVertex('A', new Dictionary<char, int>() { { 'B', 1 }, { 'C', 8 } });
            d.AddVertex('B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } });
            d.AddVertex('C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 10 } });
            d.AddVertex('D', new Dictionary<char, int>() { { 'F', 8 }, { 'H', 3 } });
            d.AddVertex('E', new Dictionary<char, int>() { { 'H', 5 } });
            d.AddVertex('F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 } });
            d.AddVertex('G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 5 }, { 'E', 6 } });
            d.AddVertex('H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } });
            d.ShortestPath('A', 'E').ForEach(x => Console.WriteLine(x));

            Console.ReadLine();
        }
    }
}
