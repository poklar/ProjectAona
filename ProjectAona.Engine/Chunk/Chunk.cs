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

        public Vector2 Position { get; private set; }

        public Tile[,] Tiles { get; private set; }

        /// <summary>
        /// The chunk width in tiles.
        /// </summary>
        public static int WidthInTiles = Core.Engine.Instance.Configuration.Chunk.WidthInTiles;

        /// <summary>
        /// The chunk height in tiles.
        /// </summary>
        public static int HeightInTiles = Core.Engine.Instance.Configuration.Chunk.HeightInTiles;

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

        public Chunk(int width, int height, Point worldQuadrant, int tileSizeInPixels)
        {
            WidthInTiles = width;
            HeightInTiles = height;
            WorldQuadrant = worldQuadrant;
            Tiles = new Tile[WidthInTiles, HeightInTiles];
            TileSizeInPixels = tileSizeInPixels;
            Position = new Vector2(WidthInPixels * worldQuadrant.X, HeightInPixels * worldQuadrant.Y);

            InitChunk();
        }

        private void InitChunk()
        {
            for (int x = 0; x < WidthInTiles; x++)
            {
                for (int y = 0; y < HeightInTiles; y++)
                {
                    Tiles[x, y] = new Tile(new Vector2(Position.X + x * TileSizeInPixels, Position.Y + y * TileSizeInPixels), TileType.None);
                }
            }
        }

        public Tile TileAt(int x, int y)
        {
            return Tiles[x, y];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            for (int x = 0; x < WidthInTiles; x++)
            {
                for (int y = 0; y < HeightInTiles; y++)
                {
                    Tiles[x, y].Draw(spriteBatch, cameraPosition);
                }
            }

            spriteBatch.DrawString(StaticData.DefaultFont, "WorldQuadrant " + WorldQuadrant, Position - cameraPosition, Color.White);
        }

        public override string ToString()
        {
            return "Chunk: " + Position;
        }
    }
}
