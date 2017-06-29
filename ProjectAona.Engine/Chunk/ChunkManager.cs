using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;

namespace ProjectAona.Engine.Chunk
{
    /// <summary>
    /// Chunk manager.
    /// </summary>
    public interface IChunkManager
    {
        /// <summary>
        /// Checks if it is in world bounds.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        bool InWorldBounds(Point worldQuadrant);

        /// <summary>
        /// Currently loaded chunks.
        /// </summary>
        /// <returns></returns>
        IChunkStorage CurrentlyLoadedChunks();
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

        /// <summary>
        /// The asset manager.
        /// </summary>
        private IAssetManager _assetManager;

        /// <summary>
        /// The tile texture.
        /// </summary>
        private ITileTexture _tileTexture;

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
            _tileTexture = (ITileTexture)Game.Services.GetService(typeof(ITileTexture));
            _camera = (ICamera)Game.Services.GetService(typeof(ICamera));
            _chunkCache = (IChunkCache)Game.Services.GetService(typeof(IChunkCache));
            _chunkStorage = (IChunkStorage)Game.Services.GetService(typeof(IChunkStorage));
            
            base.Initialize();
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
        /// Currently loaded chunks.
        /// </summary>
        /// <returns></returns>
        public IChunkStorage CurrentlyLoadedChunks()
        {
            return _chunkCache.ChunkStorage;
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
