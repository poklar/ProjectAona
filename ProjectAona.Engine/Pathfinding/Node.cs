using ProjectAona.Engine.Tiles;
using System.Collections.Generic;

namespace ProjectAona.Engine.Pathfinding
{
    public partial class Node
    {
        public Tile Tile { get; private set; }

        public Edge[] Edges { get; set; }

        public Node(Tile tile)
        {
            Tile = tile;
        }

    }

    sealed partial class Node : IHasNeighbors<Node>
    {
        public IEnumerable<Node> Neighbors
        {
            get
            {
                List<Node> nodes = new List<Node>();

                foreach (Edge edge in Edges)
                {
                    nodes.Add(edge.Node);
                }

                return nodes;
            }
        }
    }
}
