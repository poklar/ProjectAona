﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using System.Collections.Generic;

namespace ProjectAona.Engine.Chunk
{
    /// <summary>
    /// Chunk manager.
    /// </summary>
    public interface IChunkManager
    {
        /// <summary>
        /// Tile at position x, y.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns></returns>
        Tile TileAt(int x, int y);

        /// <summary>
        /// Chunk at world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        Chunk ChunkAt(Point worldQuadrant);

        /// <summary>
        /// Checks if it is in world bounds.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        bool InWorldBounds(Point worldQuadrant);

        /// <summary>
        /// Currently visible chunks.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Chunk> VisibleChunks();
    }
    /// <summary>
    /// The chunk manager.
    /// </summary>
    /// <seealso cref="Microsoft.Xna.Framework.DrawableGameComponent" />
    /// <seealso cref="ProjectAona.Engine.Chunk.IChunkManager" />
    public class ChunkManager : DrawableGameComponent, IChunkManager
    {
        /// <summary>
        /// The chunk ratio width.
        /// </summary>
        public int ChunkRatioWidth = Core.Engine.Instance.Configuration.World.MapWidth / Core.Engine.Instance.Configuration.Chunk.WidthInTiles;

        /// <summary>
        /// The chunk ratio height
        /// </summary>
        public int ChunkRatioHeight = Core.Engine.Instance.Configuration.World.MapHeight / Core.Engine.Instance.Configuration.Chunk.HeightInTiles;

        private Chunk[,] _chunks;

        /// <summary>
        /// The asset manager.
        /// </summary>
        private IAssetManager _assetManager;

        /// <summary>
        /// The camera.
        /// </summary>
        private ICamera _camera;

        /// <summary>
        /// The chunk cache.
        /// </summary>
        private IChunkCache _chunkCache;

        /// <summary>
        /// The chunk storage.
        /// </summary>
        private IChunkStorage _chunkStorage;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        public ChunkManager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            // Export service
            Game.Services.AddService(typeof(IChunkManager), this);

            _spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            // Get the services
            _assetManager = (IAssetManager)Game.Services.GetService(typeof(IAssetManager));
            _camera = (ICamera)Game.Services.GetService(typeof(ICamera));
            _chunkCache = (IChunkCache)Game.Services.GetService(typeof(IChunkCache));
            _chunkStorage = (IChunkStorage)Game.Services.GetService(typeof(IChunkStorage));

            // Initalize the chunks
            InitializeChunks();
            
            base.Initialize();
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
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, _camera.View);

            DrawAllPartiallyVisibleChunks();
            DrawCurrentlyLoadedChunksInfo();

            _spriteBatch.End();

            base.Draw(gameTime);
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
                        Tile tile = chunk.Tiles[x, y];

                        // Get the texture from the tile textures dictionary
                        //Texture2D texture = _tileTexture.TileTextures[tile.TileType];


                        // TODO: why tile.Position - _camera.Position?
                        _spriteBatch.Draw(TileTexture(tile.TileType), tile.Position - _camera.Position, Color.White);
                    }
                }

                // Draw string
                _spriteBatch.DrawString(_assetManager.DefaultFont, "WorldQuadrant " + chunk.WorldQuadrant, chunk.Position - _camera.Position, Color.White);
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
            Vector2 textPosition = Vector2.UnitY * 260;
            // Draw string
            _spriteBatch.DrawString(_assetManager.DefaultFont, "Currently loaded worldQuadrants", textPosition, Color.White);

            // Increase the position
            textPosition += textOffset;

            // For each chunk that is currently stored
            foreach (var chunk in _chunkCache.QuadrantsCurrentlyInMemory)
            {
                // Draw the chunk position
                _spriteBatch.DrawString(_assetManager.DefaultFont, chunk.ToString(), textPosition, Color.White);

                // Increase text position
                textPosition += textOffset;
            }
        }

        /// <summary>
        /// Tile at position x, y.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns></returns>
        public Tile TileAt(int x, int y)
        {
            // Check if it's in world bounds
            if (InWorldBounds(x, y))
            {
                // Get the chunk by deviding x and y by chunks in width/height times pixels of the tiles
                Chunk chunk = _chunks[x / 512, y / 512]; // TODO: That 512 hardcoed is UGLY; FIX it
                // Find the remainder of x and y and then divide it by pixels in width/height
                Tile tile = chunk.Tiles[(x % 512) / 32, (y % 512 ) / 32];

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
        public Chunk ChunkAt(Point worldQuadrant)
        {
            // Check if in it's in world bounds
            if (InWorldBounds(worldQuadrant))
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
        public IEnumerable<Chunk> VisibleChunks()
        {
            // Return visible chunks
            return _chunkCache.GetVisibleChunks(_camera.ScreenRectangle);
        }

        /// <summary>
        /// Checks if it is in world bounds.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool InWorldBounds(int x, int y)
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
        public bool InWorldBounds(Point worldQuadrant)
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
