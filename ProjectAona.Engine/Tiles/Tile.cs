using Microsoft.Xna.Framework;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.NPC;
using ProjectAona.Engine.World.TerrainObjects;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.Tiles
{
    /// <summary>
    /// The terrain tile.
    /// </summary>
    public class Tile
    {
        private float _baseTileMovementCost = 1;

        public Vector2 Position { get; set; }

        public TileType TileType { get; set; }

        public bool IsOccupied { get; set; }

        public Flora Flora { get; set; }
        
        public Wall Wall { get; set; }

        public List<Minion> Minions { get; set; }

        public IQueueable Blueprint { get; set; }

        public EnterabilityType Enterability { get; set; }

        public Stockpile Stockpile { get; set; }

        public List<IStackable> Item { get; set; }

        public float MovementCost
        {
            // TODO: Tiletypes should have movement costs (ie paths)
            get
            {
                if (Wall != null)
                    return _baseTileMovementCost * Wall.MovementCost;
                else if (Flora != null)
                    return _baseTileMovementCost * Flora.MovementCost;
                else if (Item.Count != 0)
                    return _baseTileMovementCost * Item.FirstOrDefault().MovementCost;

                return _baseTileMovementCost;
            }

            set
            {
                _baseTileMovementCost = value;
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
            Item = new List<IStackable>();
        }

        public Tile(Tile other)
        {
            Position = other.Position;
            TileType = other.TileType;
            IsOccupied = other.IsOccupied;
            Flora = other.Flora;
            Wall = other.Wall;
            Minions = other.Minions;
            Blueprint = other.Blueprint;
            Enterability = other.Enterability;
            Stockpile = other.Stockpile;
            Item = other.Item;
            MovementCost = other.MovementCost;
        }
    }
}
