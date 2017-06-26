using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Tiles;

namespace ProjectAona.Engine.Chunk
{
    public class Chunk
    {
        /// <summary>
        /// Gets the world quadrant.
        /// </summary>
        /// <value>
        /// The world quadrant.
        /// </value>
        public Point WorldQuadrant { get; private set; }

        /// <summary>
        /// Gets the position of the tile.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Gets the tiles.
        /// </summary>
        /// <value>
        /// The tiles.
        /// </value>
        public Tile[,] Tiles { get; private set; }

        /// <summary>
        /// The chunk width in tiles.
        /// </summary>
        public int WidthInTiles { get; private set; }

        /// <summary>
        /// The chunk height in tiles.
        /// </summary>
        public int HeightInTiles { get; private set; }

        /// <summary>
        /// Gets or sets the tile size in pixels.
        /// </summary>
        /// <value>
        /// The tile size in pixels.
        /// </value>
        public int TileSizeInPixels { get; set; }

        /// <summary>
        /// Gets the width in pixels.
        /// </summary>
        /// <value>
        /// The width in pixels.
        /// </value>
        public int WidthInPixels { get { return TileSizeInPixels * WidthInTiles; } }

        /// <summary>
        /// Gets the height in pixels.
        /// </summary>
        /// <value>
        /// The height in pixels.
        /// </value>
        public int HeightInPixels { get { return TileSizeInPixels * HeightInTiles; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chunk"/> class.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <param name="tileSizeInPixels">The tile size in pixels.</param>
        public Chunk(Point worldQuadrant, int tileSizeInPixels = 32)
        {
            // Setters
            WidthInTiles = Core.Engine.Instance.Configuration.Chunk.WidthInTiles;
            HeightInTiles = Core.Engine.Instance.Configuration.Chunk.HeightInTiles;
            WorldQuadrant = worldQuadrant;
            Tiles = new Tile[WidthInTiles, HeightInTiles];
            TileSizeInPixels = tileSizeInPixels;
            Position = new Vector2(WidthInPixels * worldQuadrant.X, HeightInPixels * worldQuadrant.Y);

            InitializeChunk();
        }

        /// <summary>
        /// Initializes the chunk.
        /// </summary>
        private void InitializeChunk()
        {
            for (int x = 0; x < WidthInTiles; x++)
            {
                for (int y = 0; y < HeightInTiles; y++)
                {
                    // Create a new tile
                    Tiles[x, y] = new Tile(new Vector2(Position.X + x * TileSizeInPixels, Position.Y + y * TileSizeInPixels), TileType.None);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Chunk: " + Position;
        }
    }
}
