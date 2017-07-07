using Microsoft.Xna.Framework;
using ProjectAona.Engine.World.NPC;
using ProjectAona.Engine.World.TerrainObjects;
using System.Collections.Generic;

namespace ProjectAona.Engine.Tiles
{
    /// <summary>
    /// The terrain tile.
    /// </summary>
    public class Tile
    {
        public Vector2 Position { get; set; }

        public TileType TileType { get; set; }

        public bool IsOccupied { get; set; }

        public Flora Flora { get; set; }
        
        public Wall Wall { get; set; }

        public List<Minion> Minions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Tile(Vector2 position, TileType tileType)
        {
            Position = position;
            TileType = tileType;
            IsOccupied = false;
            Minions = new List<Minion>();
        }
    }
}
