using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.Tiles
{
    /// <summary>
    /// The terrain tile.
    /// </summary>
    public class Tile
    {
        private TileType _tileType = TileType.None;
        private Vector2 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Tile(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the type of the tile.
        /// </summary>
        /// <value>
        /// The type of the tile.
        /// </value>
        public TileType TileType
        {
            get { return _tileType; }
            set { _tileType = value; }
        }
    }
}
