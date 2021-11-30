using System.Collections.Generic;
using System.Linq;

namespace EightPuzzle
{
    public class BreadthFirstSearch : ISearch
    {
        
        private class Node
        {
            public readonly (State s, Direction d) State;
            public readonly Node? Previous;
        
            public Node(State state, Direction direction = Direction.None) 
                : this((state, direction)) {}

            public Node((State s, Direction d) state, Node? previous = null)
            {
                State = state;
                Previous = previous;
            }

            public List<Node> GenerateNeighbors()
            {
                var list = new List<Node>(4);
            
                foreach (var state in State.s.GenerateNeighborStates())
                {
                    list.Add(new Node(state, this));
                }

                return list;
            }

        }
        
        private readonly HashSet<State> _visited = new HashSet<State>();
        private readonly Queue<Node> _states = new Queue<Node>();
        public int Iterations { get; private set; }

        public List<(State s, Direction d)> Solve(State initial)
        {
            Iterations = 0;
            
            _states.Enqueue(new Node(initial));
            _visited.Add(initial);
            var last = Search();
            var list = new List<(State s, Direction d)>();
            while (last is not null)
            {
                list.Add(last.State);
                last = last.Previous;
            }
            list.Reverse();
            
            _states.Clear();
            _visited.Clear();
            
            return list;
        }

        private Node? Search()
        {
            while (_states.Any())
            {
                ++Iterations;
                var front = _states.Dequeue();
                if (front.State.s.Solved())
                {
                    return front;
                }
                
                foreach (var node in front.GenerateNeighbors())
                {
                    if (!_visited.Contains(node.State.s))
                    {
                        _visited.Add(node.State.s);
                        _states.Enqueue(node);
                    }
                }
            }

            return null;
        }
    }
}