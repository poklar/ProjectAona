// All credit goes to Martin "quill18" Glaude: http://quill18.com

using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Tiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProjectAona.Engine.Pathfinding
{
    public class Graph
    {
        public Dictionary<Tile, Node> Nodes { get; private set; }

        public Graph()
        {
            Nodes = new Dictionary<Tile, Node>();

            // Loop through all tiles of the world and create a node for each tile
            foreach (Chunk chunk in ChunkManager.Chunks())
            {
                for (int x = 0; x < chunk.WidthInTiles; x++)
                {
                    for (int y = 0; y < chunk.HeightInTiles; y++)
                    {
                        Tile tile = chunk.TileAt(x, y);
                        Node node = new Node(tile);
                        Nodes.Add(tile, node);
                    }
                }
            }
            
            int edgeCount = 0;

            // Loop through all nodes again and create edges for neighbors
            foreach (Tile tile in Nodes.Keys)
            {
                Node node = Nodes[tile];

                List<Edge> edges = new List<Edge>();

                // Get neighbors from the current tile
                Tile[] neighbors = ChunkManager.TileNeighbors(tile, true); // Some of the array spots could be null

                for (int i = 0; i < neighbors.Length; i++)
                {
                    // If neighbor exists, is walkable and doesn't require clipping a corner --> create an edge
                    if (neighbors[i] != null && neighbors[i].MovementCost > 0 && !IsClippingCorner(tile, neighbors[i]))
                    {                  
                        Edge edge = new Edge();
                        edge.Cost = neighbors[i].MovementCost;
                        edge.Node = Nodes[neighbors[i]];

                        // Add edge to temporary and growable list
                        edges.Add(edge);

                        // Debugging
                        edgeCount++;
                    }
                }

                node.Edges = edges.ToArray();
            }

            Debug.WriteLine("Edges created: " + edgeCount);
        } 

        private bool IsClippingCorner(Tile currentTile, Tile neighbor)
        {
            // If movement from current tile to neighboring tile is diagonal, check if we aren't clipping 

            int dX = (int)currentTile.Position.X - (int)neighbor.Position.X;
            int dY = (int)currentTile.Position.Y - (int)neighbor.Position.Y;

            // If diagonal
            if (Math.Abs(dX) + Math.Abs(dY) == 64)
            {
                if (ChunkManager.TileAtWorldPosition((int)currentTile.Position.X - dX, (int)currentTile.Position.Y) != null &&
                    ChunkManager.TileAtWorldPosition((int)currentTile.Position.X - dX, (int)currentTile.Position.Y).MovementCost >= 10000) // East/West is unwalkable
                    return true;

                if (ChunkManager.TileAtWorldPosition((int)currentTile.Position.X, (int)currentTile.Position.Y + dY) != null &&
                    ChunkManager.TileAtWorldPosition((int)currentTile.Position.X, (int)currentTile.Position.Y + dY).MovementCost >= 10000) // North/South is unwalkable
                    return true;

                if (ChunkManager.TileAtWorldPosition((int)currentTile.Position.X + dX, (int)currentTile.Position.Y) != null &&
                    ChunkManager.TileAtWorldPosition((int)currentTile.Position.X + dX, (int)currentTile.Position.Y).MovementCost >= 10000) // East/West is unwalkable
                    return true;

                if (ChunkManager.TileAtWorldPosition((int)currentTile.Position.X, (int)currentTile.Position.Y - dY) != null &&
                    ChunkManager.TileAtWorldPosition((int)currentTile.Position.X, (int)currentTile.Position.Y - dY).MovementCost >= 10000) // North/South is unwalkable
                    return true;

                // If it reaches here, it's diagonal, but not clipping
            }

            // If it's here, it's either not clipping, or not diagonal
            return false;
        }
    }
}
