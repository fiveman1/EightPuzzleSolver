using System.Collections.Generic;

namespace EightPuzzle
{
    public interface ISearch
    {
        
        int Iterations { get; }
        public List<(State s, Direction d)> Solve(State initial);
    }
}