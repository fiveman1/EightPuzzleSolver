using System;
using System.Collections.Generic;
using System.Text;

namespace EightPuzzle
{

    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
    
    public static class DirectionExtension
    {
        public static Direction Reverse(this Direction direction) => direction switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            _ => Direction.None
        };
    }
    public class State
    {
        /*
         * 0 1 2
         * 3 4 5
         * 6 7 8
         */
        private readonly byte[] _array = new byte[9];
        
        private int _empty;

        public State(State other)
        {
            _empty = other._empty;
            other._array.CopyTo(_array, 0);
        }

        public State(byte[] array)
        {
            for (var i = 0; i < 9; ++i)
            {
                var x = array[i];
                _array[i] = x;
                if (x == 0)
                {
                    _empty = i;
                }
            }
        }

        private static readonly int[] Hashes = new int[97];

        public override int GetHashCode()
        {
            var hash = 19;
            for (var i = 0; i < 9; ++i)
            {
                hash = hash * 33 + _array[i] * (i+1);
            }

            Hashes[Math.Abs(hash % 97)] += 1;
            return hash;
        }

        public override bool Equals(object? obj)
        {
            var state = obj as State;
            if (state is null)
            {
                return false;
            }
            
            for (var i = 0; i < 8; ++i)
            {
                if (state._array[i] != _array[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool Solved()
        {
            byte x = 1;
            for (var i = 0; i < 8; ++i)
            {
                if (_array[i] != x++)
                {
                    return false;
                }
            }

            return true;
        }

        public State Left()
        {
            var state = new State(this);
            var i = _empty - 1;
            (state._array[_empty], state._array[i]) = (state._array[i], state._array[_empty]);
            state._empty = i;
            return state;
        }

        public State Right()
        {
            var state = new State(this);
            var i = _empty + 1;
            (state._array[_empty], state._array[i]) = (state._array[i], state._array[_empty]);
            state._empty = i;
            return state;
        }
        
        public State Up()
        {
            var state = new State(this);
            var i = _empty - 3;
            (state._array[_empty], state._array[i]) = (state._array[i], state._array[_empty]);
            state._empty = i;
            return state;
        }
        
        public State Down()
        {
            var state = new State(this);
            var i = _empty + 3;
            (state._array[_empty], state._array[i]) = (state._array[i], state._array[_empty]);
            state._empty = i;
            return state;
        }

        public List<(State s, Direction d)> GenerateNeighborStates()
        {
            var list = new List<(State s, Direction d)>(4);
            
            if (_empty % 3 != 0)
            {
                list.Add((Left(), Direction.Left));
            }
            
            if (_empty % 3 != 2)
            {
                list.Add((Right(), Direction.Right));
            }
            
            if (_empty > 2)
            {
                list.Add((Up(), Direction.Up));
            }

            if (_empty < 6)
            {
                list.Add((Down(), Direction.Down));
            }

            return list;
        }

        public static State CreateStartState()
        {
            return new State(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 0});
        }

        public static State CreateRandomState(int shuffles = 100)
        {
            var random = new Random();
            var state = CreateStartState();
            for (var i = 0; i < shuffles; ++i)
            {
                var list = state.GenerateNeighborStates();
                state = list[random.Next(list.Count)].s;
            }

            return state;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            for (var i = 0; i < 3; ++i)
            {
                builder.Append('[');
                builder.Append(_array[i * 3]);
                builder.Append(", ");
                builder.Append(_array[i * 3 + 1]);
                builder.Append(", ");
                builder.Append(_array[i * 3 + 2]);
                builder.Append("]\n");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(']');
            return builder.ToString();
        }

        public Direction DirectionTo(State other)
        {
            if (_empty == other._empty + 1)
            {
                return Direction.Right;
            }
            if (_empty == other._empty - 1)
            {
                return Direction.Left;
            }
            if (_empty == other._empty + 3)
            {
                return Direction.Up;
            }
            if (_empty == other._empty - 3)
            {
                return Direction.Down;
            }

            return Direction.None;
        }

        public int ManhattanDistance()
        {
            var sum = 0;
            for (var i = 0; i < 9; ++i)
            {
                sum += ManhattanHelper(_array[i], i);
            }
            return sum;
        }

        private static readonly Dictionary<byte, (int, int)> NumToCoord = new Dictionary<byte, (int, int)>
        {
            { 1, (0, 0) },
            { 2, (1, 0) },
            { 3, (2, 0) },
            { 4, (0, 1) },
            { 5, (1, 1) },
            { 6, (2, 1) },
            { 7, (0, 2) },
            { 8, (1, 2) },
            { 0, (2, 2) }
        };

        private int ManhattanHelper(byte num, int idx)
        {
            var x = idx % 3;
            var y = idx / 3;
            var (nx, ny) = NumToCoord[num];
            return Math.Abs(x - nx) + Math.Abs(y - ny);
        }
    }
}