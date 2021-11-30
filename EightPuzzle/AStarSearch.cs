using System;
using System.Collections.Generic;

namespace EightPuzzle
{
    public class AStarSearch : ISearch
    {
        
        private class Node
        {
            public (State s, Direction d) State;
            public Node? Parent;
            public int Heuristic = 0;
            public int Steps = 0;
        
            public Node(State state, Direction direction = Direction.None) 
                : this((state, direction)) {}

            public Node((State s, Direction d) state, Node? parent = null)
            {
                State = state;
                Parent = parent;
            }

            public List<Node> GenerateNeighbors()
            {
                var list = new List<Node>(4);
            
                foreach (var state in State.s.GenerateNeighborStates())
                {
                    var node = new Node(state, this);
                    node.Steps = Steps + 1;
                    list.Add(node);
                }

                return list;
            }
            
            public override bool Equals(object? obj)
            {
                var other = obj as Node;
                return other is not null && State.s.Equals(other.State.s);
            }

            public override int GetHashCode()
            {
                return State.s.GetHashCode();
            }

        }

        private readonly PriorityQueue<Node, int> _open = new PriorityQueue<Node, int>();
        private readonly Dictionary<Node, int> _openValues = new Dictionary<Node, int>();
        private readonly Dictionary<Node, int> _closed = new Dictionary<Node, int>();
        
        public int Iterations { get; private set; }

        public List<(State s, Direction d)> Solve(State initial)
        {
            Iterations = 0;
            AddToQueue(new Node(initial), initial.ManhattanDistance());
            var curr = Search();
            var list = new List<(State s, Direction d)>();
            while (curr is not null)
            {
                list.Add(curr.State);
                curr = curr.Parent;
            }
            list.Reverse();
            _open.Clear();
            _openValues.Clear();
            _closed.Clear();

            return list;
        }

        private Node? Search()
        {
            while (_open.Count > 0)
            {
                ++Iterations;
                var front = _open.Dequeue();
                if (_openValues[front] < front.Heuristic) continue; 
                _closed[front] = front.Heuristic;
                if (front.State.s.Solved())
                {
                    return front;
                }

                foreach (var node in front.GenerateNeighbors())
                {
                    node.Heuristic = node.Steps + node.State.s.ManhattanDistance();
                    if (_openValues.ContainsKey(node))
                    {
                        var heuristic = _openValues[node];
                        if (heuristic < node.Heuristic) continue;
                    }
                    if (_closed.ContainsKey(node))
                    {
                        var heuristic = _closed[node];
                        if (heuristic < node.Heuristic) continue;
                        _closed.Remove(node);
                    }
                    AddToQueue(node);
                }
            }
            return null;
        }

        private void AddToQueue(Node node, int heuristic)
        {
            node.Heuristic = heuristic;
            _open.Enqueue(node, heuristic);
            _openValues[node] = heuristic;
        }
        
        private void AddToQueue(Node node)
        {
            _open.Enqueue(node, node.Heuristic);
            _openValues[node] = node.Heuristic;
        }
    }
}