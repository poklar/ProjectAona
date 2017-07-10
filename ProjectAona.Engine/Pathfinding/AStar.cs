// Credit goes to: https://blogs.msdn.microsoft.com/ericlippert/tag/astar/

using ProjectAona.Engine.Tiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectAona.Engine.Pathfinding
{
    public class AStar
    {
        private Stack<Tile> _path = null;

        public AStar(Tile startTile, Tile destinationTile)
        {
            if (Core.Engine.Graph == null)
            {
                Core.Engine.Graph = new Graph();
                Debug.WriteLine("new graph");
            }

            Node start = Core.Engine.Graph.Nodes[startTile];
            Node destination = Core.Engine.Graph.Nodes[destinationTile];

            Func<Node, Node, float> distance = (node1, node2) => node1.Edges.Cast<Edge>().Single(edge => edge.Node.Tile == node2.Tile).Cost;
            Func<Node, float> manhattenEstimation = node => Math.Abs(node.Tile.Position.X - destination.Tile.Position.X) + Math.Abs(node.Tile.Position.Y - destination.Tile.Position.Y);

            var closed = new HashSet<Node>();
            var queue = new PriorityQueue<float, Path<Node>>();

            queue.Enqueue(0, new Path<Node>(start));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                    continue;

                if (path.LastStep.Equals(destination))
                {
                    ReconstructPath(path);
                    return;
                }

                closed.Add(path.LastStep);

                foreach (Node node in path.LastStep.Neighbors) // TODO: Grab nodes from graph?? and remove partial? try
                {
                    float dist = distance(path.LastStep, node);

                    var newPath = path.AddStep(node, dist);

                    queue.Enqueue(newPath.TotalCost + manhattenEstimation(node), newPath);
                }
            }
        }

        private void ReconstructPath(Path<Node> path)
        {
            Stack<Tile> stack = new Stack<Tile>();
                
            foreach (Path<Node> p in path)
                stack.Push(p.LastStep.Tile);

            _path = stack;
        }

        public Tile Pop()
        {
            if (_path == null)
                return null;

            if (_path.Count <= 0)
            {
                Debug.WriteLine("Astar.Pop :: _path.Count <= 0"); // TODO: Throw error
                return null;
            }

            return _path.Pop();
        }

        public int Length()
        {
            if (_path == null)
                return 0;

            return _path.Count;
        }

        public Stack<Tile> GetStack()
        {
            return _path;
        }
    }
}
