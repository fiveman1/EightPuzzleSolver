using System.Collections.Generic;
using System.Linq;

namespace EightPuzzle
{
    
    
    public class BidirectionalSearch : ISearch
    {
        
        private class Node
        {
            public readonly (State s, Direction d) State;
        
            public Node(State state, Direction direction = Direction.None) 
                : this((state, direction)) {}

            public Node((State s, Direction d) state)
            {
                State = state;
            }

            public List<Node> GenerateNeighbors()
            {
                var list = new List<Node>(4);
            
                foreach (var state in State.s.GenerateNeighborStates())
                {
                    list.Add(new Node(state));
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
        
        private readonly Dictionary<Node, Node?> _startVisited = new Dictionary<Node, Node?>();
        private readonly Dictionary<Node, Node?> _endVisited = new Dictionary<Node, Node?>();
        private readonly Queue<Node> _startQueue = new Queue<Node>();
        private readonly Queue<Node> _endQueue = new Queue<Node>();
        public int Iterations { get; private set; }

        public List<(State s, Direction d)> Solve(State initial)
        {
            Iterations = 0;
            
            var start = new Node(initial);
            _startVisited[start] = null;
            _startQueue.Enqueue(start);
            var end = new Node(State.CreateStartState());
            _endVisited[end] = null;
            _endQueue.Enqueue(end);

            var middle = Search();
            
            // Combine start and end searches
            var list = new List<(State s, Direction d)>();
            
            // Start by going backwards from the middle
            var curr = middle;
            while (curr is not null)
            {
                list.Add(curr.State);
                curr = _startVisited[curr];
            }
            // Since the list is backwards, reverse it
            list.Reverse();
            
            // Now go forwards
            curr = middle;
            
            // The direction to move is stored in the previous node
            var lastDirection = Direction.None;
            // Also, skip the first node (we already did that in the last part)
            if (curr is not null)
            {
                var next = _endVisited[curr];
                if (next is not null)
                {
                    // We don't have the direction stored for how to move from the middle to the next node, but
                    // we can just calculate it
                    lastDirection = curr.State.s.DirectionTo(next.State.s);
                }
                curr = next;
            }
            // Add the nodes from middle to end to the list (forwards order this time)
            while (curr is not null)
            {
                // We have to reverse the direction because we got here going backwards (from end to middle)
                list.Add((curr.State.s, lastDirection.Reverse()));
                lastDirection = curr.State.d;
                curr = _endVisited[curr];
            }
            
            _startQueue.Clear();
            _startVisited.Clear();
            _endQueue.Clear();
            _endVisited.Clear();

            return list;
        }

        private Node? Search()
        {
            var startHasItems = true;
            var endHasItems = true;
            while (startHasItems || endHasItems)
            {
                startHasItems = _startQueue.Any();
                if (startHasItems)
                {
                    ++Iterations;
                    var front = _startQueue.Dequeue();

                    if (_endVisited.ContainsKey(front))
                    {
                        return front;
                    }

                    foreach (var node in front.GenerateNeighbors())
                    {
                        if (!_startVisited.ContainsKey(node))
                        {
                            _startVisited[node] = front;
                            _startQueue.Enqueue(node);
                        }
                    }
                }

                endHasItems = _endQueue.Any();
                if (endHasItems)
                {
                    ++Iterations;
                    var front = _endQueue.Dequeue();

                    if (_startVisited.ContainsKey(front))
                    {
                        return front;
                    }

                    foreach (var node in front.GenerateNeighbors())
                    {
                        if (!_endVisited.ContainsKey(node))
                        {
                            _endVisited[node] = front;
                            _endQueue.Enqueue(node);
                        }
                    }
                }
            }

            return null;
        }
        
    }
}