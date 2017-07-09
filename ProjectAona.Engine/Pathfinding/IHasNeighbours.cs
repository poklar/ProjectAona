using System.Collections.Generic;

namespace ProjectAona.Engine.Pathfinding
{
    public interface IHasNeighbors<N>
    {
        IEnumerable<N> Neighbors { get; }
    }
}