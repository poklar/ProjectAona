using Microsoft.Xna.Framework;
using ProjectAona.Engine.Jobs;
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
        private const float _baseTileMovementCost = 1;

        public Vector2 Position { get; set; }

        public TileType TileType { get; set; }

        public bool IsOccupied { get; set; }

        public Flora Flora { get; set; }
        
        public Wall Wall { get; set; }

        public List<Minion> Minions { get; set; }

        public IQueueable Blueprint { get; set; }

        public EnterabilityType Enterability { get; set; }

        public float MovementCost
        {
            // TODO: Tiletypes should have movement costs (ie paths)
            get
            {
                if (Wall != null)
                    return _baseTileMovementCost * Wall.MovementCost;
                else if (Flora != null)
                    return _baseTileMovementCost * Flora.MovementCost;
                // TODO: Add furniture

                return _baseTileMovementCost;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Tile(Vector2 position, TileType tileType)
        {
            Position = position;
            TileType = tileType;
            IsOccupied = false;
            Enterability = EnterabilityType.IsEnterable;
            Minions = new List<Minion>();
        }
    }
}
