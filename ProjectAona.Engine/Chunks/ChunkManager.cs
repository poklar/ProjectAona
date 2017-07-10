using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using System.Collections.Generic;

namespace ProjectAona.Engine.Chunks
{
    /// <summary>
    /// The chunk manager.
    /// </summary>
    public class ChunkManager
    {
        /// <summary>
        /// The chunk ratio width.
        /// </summary>
        public static int ChunkRatioWidth = Core.Engine.Instance.Configuration.World.MapWidth / Core.Engine.Instance.Configuration.Chunk.WidthInTiles;

        /// <summary>
        /// The chunk ratio height
        /// </summary>
        public static int ChunkRatioHeight = Core.Engine.Instance.Configuration.World.MapHeight / Core.Engine.Instance.Configuration.Chunk.HeightInTiles;

        /// <summary>
        /// All the chunks.
        /// </summary>
        private static Chunk[,] _chunks;

        /// <summary>
        /// The game.
        /// </summary>
        private Game _game;

        /// <summary>
        /// The camera.
        /// </summary>
        private static Camera _camera;

        /// <summary>
        /// The chunk cache.
        /// </summary>
        private static ChunkCache _chunkCache;

        /// <summary>
        /// The asset manager.
        /// </summary>
        private AssetManager _assetManager;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        public ChunkManager(Game game, SpriteBatch spriteBatch, Camera camera, AssetManager assetManager)
        {
            _game = game;
            _spriteBatch = spriteBatch;
            _camera = camera;
            _assetManager = assetManager;
            _chunkCache = new ChunkCache();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            // Initalize the chunks
            InitializeChunks();
            _chunkCache.LoadChunks(_chunks, ChunkRatioWidth, ChunkRatioHeight);
        }

        /// <summary>
        /// Initializes the chunks.
        /// </summary>
        private void InitializeChunks()
        {
            // Initialize the array
            _chunks = new Chunk[ChunkRatioWidth, ChunkRatioHeight];

            // For every width ratio..
            for (int x = 0; x < ChunkRatioWidth; x++)
            {
                // For every height ratio..
                for (int y = 0; y < ChunkRatioHeight; y++)
                {
                    // Create a new chunk
                    _chunks[x, y] = new Chunk(new Point(x, y));
                }
            }
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);
            DrawAllPartiallyVisibleChunks();
            _spriteBatch.End();

            _spriteBatch.Begin();
            DrawCurrentlyLoadedChunksInfo();
            _spriteBatch.End();
        }

        /// <summary>
        /// Draws all partially visible chunks.
        /// </summary>
        private void DrawAllPartiallyVisibleChunks()
        {
            // For each chunk in the visible chunk list
            foreach (Chunk chunk in _chunkCache.GetVisibleChunks(_camera.ScreenRectangle))
            {
                for (int x = 0; x < chunk.WidthInTiles; x++)
                {
                    for (int y = 0; y < chunk.HeightInTiles; y++)
                    {
                        // Get the tile
                        Tile tile = chunk.TileAt(x, y);

                        // Draw sprite
                        _spriteBatch.Draw(TileTexture(tile.TileType), tile.Position, Color.White);
                    }
                }

                // Draw string
                _spriteBatch.DrawString(_assetManager.DefaultFont, "WorldQuadrant " + chunk.WorldQuadrant, chunk.Position, Color.White);
            }
        }



        /// <summary>
        /// Draws the currently loaded chunks information.
        /// </summary>
        private void DrawCurrentlyLoadedChunksInfo()
        {
            // Offset for text
            Vector2 textOffset = Vector2.UnitY * 20;
            // Position of the text
            Vector2 textPosition = Vector2.UnitY * 0;

            int chunkCounter = 0;

            // For each chunk that is currently stored
            foreach (var chunk in _chunkCache.QuadrantsCurrentlyInMemory)
                chunkCounter++;

            // Draw string
            _spriteBatch.DrawString(_assetManager.DefaultFont, "Loaded chunks: " + chunkCounter, textPosition, Color.White);


        }

        /// <summary>
        /// Tile at position world position x, y.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns></returns>
        public static Tile TileAtWorldPosition(int x, int y)
        {
            int tileWidth = Core.Engine.Instance.Configuration.Chunk.WidthInTiles * 32;
            int tileHeight = Core.Engine.Instance.Configuration.Chunk.HeightInTiles * 32;

            // Check if it's in world bounds
            if (InWorldBounds(x, y))
            {
                // Get the chunk by deviding x and y by chunks in width/height times pixels of the tiles
                Chunk chunk = _chunks[x / tileWidth, y / tileHeight]; 
                // Find the remainder of x and y and then divide it by pixels in width/height
                Tile tile = chunk.TileAt((x % tileWidth) / 32, (y % tileHeight) / 32);

                // Return the tile
                return tile;
            }

            // Not in bounds, return null
            return null;
        }

        /// <summary>
        /// Chunk at world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        public static Chunk ChunkAt(Point worldQuadrant)
        {
            // Check if in it's in world bounds
            if (ChunkInWorldBounds(worldQuadrant))
            {
                // Return the chunk
                return _chunks[worldQuadrant.X, worldQuadrant.Y];
            }
            // If it doesn't exist
            else
                return null;
        }

        /// <summary>
        /// Currently visible chunks.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Chunk> VisibleChunks()
        {
            // Return visible chunks
            return _chunkCache.GetVisibleChunks(_camera.ScreenRectangle);
        }

        /// <summary>
        /// Checks if tile is in world bounds.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns></returns>
        public static bool InWorldBounds(int x, int y)
        {
            // If x/y less then 0 or x/y are bigger than mapwidth/height times pixels, out of bounds, return false
            if (x < 0 || y < 0 || x >= Core.Engine.Instance.Configuration.World.MapWidth * 32 || y >= Core.Engine.Instance.Configuration.World.MapHeight * 32)
                return false;

            // Otherwise return true
            return true;
        }

        /// <summary>
        /// Checks if it is in world bounds.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        public static bool ChunkInWorldBounds(Point worldQuadrant)
        {
            // Check if the world quadrant is in between the chunk ratio 
            if (worldQuadrant.X < ChunkRatioWidth && worldQuadrant.X >= 0 && 
                worldQuadrant.Y < ChunkRatioHeight && worldQuadrant.Y >= 0)
            {
                // Return true
                return true;
            }
            // If out of bounds
            else
                return false;
        }

        public static Chunk[,] Chunks()
        {
            return _chunks;
        }

        // TODO: Make a tile storage/manager??
        public static Tile[] TileNeighbors(Tile tile, bool diagonal = false)
        {
            Tile[] neighbors;

            if (!diagonal)
                neighbors = new Tile[4]; // Tile order: N E S W
            else
                neighbors = new Tile[8]; // Tile order: N E S W NE SE SW NW

            Tile neighbor;

            // Could be null, but that's okay.
            neighbor = TileAtWorldPosition((int)tile.Position.X, (int)tile.Position.Y - 32); // N
            neighbors[0] = neighbor;  
            neighbor = TileAtWorldPosition((int)tile.Position.X + 32, (int)tile.Position.Y); // E
            neighbors[1] = neighbor;  
            neighbor = TileAtWorldPosition((int)tile.Position.X, (int)tile.Position.Y + 32); // S
            neighbors[2] = neighbor;  
            neighbor = TileAtWorldPosition((int)tile.Position.X - 32, (int)tile.Position.Y); // W
            neighbors[3] = neighbor;  

            if (diagonal)
            {
                neighbor = TileAtWorldPosition((int)tile.Position.X + 32, (int)tile.Position.Y - 32); //NE
                neighbors[4] = neighbor;  
                neighbor = TileAtWorldPosition((int)tile.Position.X + 32, (int)tile.Position.Y + 32); // SE
                neighbors[5] = neighbor;  
                neighbor = TileAtWorldPosition((int)tile.Position.X - 32, (int)tile.Position.Y + 32); // SW
                neighbors[6] = neighbor;  
                neighbor = TileAtWorldPosition((int)tile.Position.X - 32, (int)tile.Position.Y - 32); // NW
                neighbors[7] = neighbor;  
            }

            return neighbors;
        }

        /// <summary>
        /// Gets the texture from the tile type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        // TODO: REMOVE, a texture atlas will be added later
        private Texture2D TileTexture(TileType type)
        {
            // Get the corresponding texture
            switch (type)
            {
                case TileType.LightGrass: return _assetManager.LightGrassTile;
                case TileType.DarkGrass: return _assetManager.DarkGrassTile;
                case TileType.Stone: return _assetManager.StoneTile;
                case TileType.Water: return _assetManager.WaterTile;
                default: return null;
            }
        }
    }
}
