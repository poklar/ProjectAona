using Microsoft.Xna.Framework;
using ProjectAona.Engine.World.TerrainObjects;

namespace ProjectAona.Engine.Tiles
{
    /// <summary>
    /// The terrain tile.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the type of the tile.
        /// </summary>
        /// <value>
        /// The type of the tile.
        /// </value>
        public TileType TileType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is occupied by an object.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the tile is occupied; otherwise, <c>false</c>.
        /// </value>
        public bool IsOccupied { get; set; }

        /// <summary>
        /// Gets or sets the flora.
        /// </summary>
        /// <value>
        /// The flora.
        /// </value>
        public Flora Flora { get; set; }

        /// <summary>
        /// Gets or sets the wall.
        /// </summary>
        /// <value>
        /// The wall.
        /// </value>
        public Wall Wall { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Tile(Vector2 position, TileType tileType)
        {
            Position = position;
            TileType = tileType;
            IsOccupied = false;
        }
    }
}
