using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EightPuzzle
{
    class Program
    {
        public static void Main(string[] args)
        {
            /*var initial = State.CreateRandomState(200);
            
            var bfs = new BreadthFirstSearch();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var list = bfs.Solve(initial);
            stopwatch.Stop();
            /*foreach (var (s, d) in list)
            {
                Console.WriteLine(d);
                Console.WriteLine(s);
            }#1#
            Console.WriteLine("BFS:");
            Console.WriteLine($"Nodes searched: {bfs.Iterations}");
            Console.WriteLine($"Length: {list.Count}");
            Console.WriteLine($"Time spent: {stopwatch.ElapsedMilliseconds} ms");
            
            var bi = new BidirectionalSearch();
            stopwatch.Restart();
            list = bi.Solve(initial);
            stopwatch.Stop();
            /*foreach (var (s, d) in list)
            {
                Console.WriteLine(d);
                Console.WriteLine(s);
            }#1# 
            Console.WriteLine("Bi-directional:");
            Console.WriteLine($"Nodes searched: {bi.Iterations}");
            Console.WriteLine($"Length: {list.Count}");
            Console.WriteLine($"Time spent: {stopwatch.ElapsedMilliseconds} ms");
            
            var astar = new AStarSearch();
            stopwatch.Restart();
            list = astar.Solve(initial);
            stopwatch.Stop();
            /*foreach (var (s, d) in list)
            {
                Console.WriteLine(d);
                Console.WriteLine(s);
            }#1#
            Console.WriteLine("A*:");
            Console.WriteLine($"Nodes searched: {astar.Iterations}");
            Console.WriteLine($"Length: {list.Count}");
            Console.WriteLine($"Time spent: {stopwatch.ElapsedMilliseconds} ms");*/
            var list = new List<(ISearch, string)>();
            list.Add((new BidirectionalSearch(), "Bi-directional"));
            list.Add((new AStarSearch(), "A*"));
            Tester(list);
        }

        public static void Tester(List<(ISearch, string)> searches, int runs = 5)
        {
            var stopwatch = new Stopwatch();
            for (var i = 0; i < runs; ++i)
            {
                var initial = State.CreateRandomState(200);
                foreach (var (search, name) in searches)
                {
                    stopwatch.Restart();
                    var list = search.Solve(initial);
                    stopwatch.Stop();
                    Console.WriteLine($"{name}:");
                    Console.WriteLine($"Nodes searched: {search.Iterations}");
                    Console.WriteLine($"Length: {list.Count}");
                    Console.WriteLine($"Time spent: {stopwatch.ElapsedMilliseconds} ms\n");
                }
            }
        }
    }
}