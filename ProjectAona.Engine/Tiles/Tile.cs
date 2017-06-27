using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;

namespace ProjectAona.Engine.Tiles
{
    /// <summary>
    /// The terrain tile.
    /// </summary>
    public class Tile
    {
        private TileType _tileType;
        private Vector2 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public Tile(Vector2 position, TileType tileType)
        {
            _position = position;
            _tileType = tileType;
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

        /// <summary>
        /// Tiles the color of the type.
        /// </summary>
        /// <param name="tileType">Type of the tile.</param>
        /// <returns></returns>
        // TODO: Just for testing purposes, will be removed later
        public Color TileTypeColor(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Grass: return Color.Green;
                case TileType.Dirt: return Color.SaddleBrown;
                case TileType.Water: return Color.Blue;
                case TileType.Marsh: return Color.DarkGreen;
                case TileType.Stone: return Color.Gray;
                case TileType.Tree: return Color.GreenYellow;
                case TileType.Tree2: return Color.Orange;
                case TileType.Bush: return Color.Black;
                case TileType.Cave: return Color.IndianRed;
                case TileType.StoneOre: return Color.Aqua;
                case TileType.IronOre: return Color.Red;
                case TileType.Coal: return Color.Black;
                case TileType.None: return Color.Pink;
                default: return Color.Pink; //to make undefined tiles stand out
            }
        }
    }
}
