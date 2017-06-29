using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.TerrainObjects;
using System;
using System.Collections.Generic;

namespace ProjectAona.Engine.World
{
    /// <summary>
    /// The terrain manager interface.
    /// </summary>
    public interface ITerrainManager
    {
        /// <summary>
        /// Gets the flora objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The flora objects.
        /// </value>
        Dictionary<Vector2, Flora> FloraObjects { get; }

        /// <summary>
        /// Gets the mineral objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The mineral objects.
        /// </value>
        Dictionary<Vector2, Mineral> MineralObjects { get; }

        /// <summary>
        /// Adds a mineral.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tile">The tile.</param>
        void AddMineral(MineralType type, Tile tile);

        /// <summary>
        /// Removes a mineral.
        /// </summary>
        /// <param name="mineral">The mineral.</param>
        /// <param name="tile">The tile.</param>
        void RemoveMineral(Mineral mineral, Tile tile);

        /// <summary>
        /// Adds a flora.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tile">The tile.</param>
        void AddFlora(FloraType type, Tile tile);

        /// <summary>
        /// Removes a flora.
        /// </summary>
        /// <param name="flora">The flora.</param>
        /// <param name="tile">The tile.</param>
        void RemoveFlora(Flora flora, Tile tile);
    }

    /// <summary>
    /// The terrain manager. 
    /// </summary>
    /// <seealso cref="Microsoft.Xna.Framework.DrawableGameComponent" />
    /// <seealso cref="ProjectAona.Engine.World.ITerrainManager" />
    public class TerrainManager : DrawableGameComponent, ITerrainManager
    {
        /// <summary>
        /// Gets the flora objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The flora objects.
        /// </value>
        public Dictionary<Vector2, Flora> FloraObjects { get; private set; }

        /// <summary>
        /// Gets the mineral objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The mineral objects.
        /// </value>
        public Dictionary<Vector2, Mineral> MineralObjects { get; private set; }

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// The asset manager.
        /// </summary>
        private IAssetManager _assetManager;

        /// <summary>
        /// The chunk manager.
        /// </summary>
        private IChunkManager _chunkManager;

        /// <summary>
        /// The camera.
        /// </summary>
        private ICamera _camera;

        public TerrainManager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            // Export service
            Game.Services.AddService(typeof(ITerrainManager), this);

            // Setters
            _spriteBatch = spriteBatch;

            FloraObjects = new Dictionary<Vector2, Flora>();
            MineralObjects = new Dictionary<Vector2, Mineral>();
        }

        public override void Initialize()
        {
            // Get services
            _assetManager = (IAssetManager)Game.Services.GetService(typeof(IAssetManager));
            _chunkManager = (IChunkManager)Game.Services.GetService(typeof(IChunkManager));
            _camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            base.Initialize();
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Draw(GameTime gameTime)
        {
            // Start drawing
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);

            DrawTerrainObjects();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws the terrain objects.
        /// </summary>
        private void DrawTerrainObjects()
        {
            // Get all the currently loaded chunks
            IChunkStorage loadedChunks = _chunkManager.CurrentlyLoadedChunks();

            // For each loaded chunks
            foreach (var chunk in loadedChunks.Values)
            {
                // Loop through each tile
                for (int x = 0; x < chunk.WidthInTiles; x++)
                {
                    for (int y = 0; y < chunk.HeightInTiles; y++)
                    {
                        // Get the position from the tile
                        Vector2 position = chunk.Tiles[x, y].Position;

                        // If a flora object is positioned on the same as the tile
                        if (FloraObjects.ContainsKey(position))
                        {
                            // Get that flora object
                            Flora flora = FloraObjects[position];

                            // And draw it 
                            _spriteBatch.Draw(FloraTexture(flora.FloraType), flora.Position - _camera.Position, Color.White);
                        }

                        // If a mineral object is positioned on the same as the tile
                        if (MineralObjects.ContainsKey(position))
                        {
                            // Get that mineral object
                            Mineral mineral = MineralObjects[position];

                            // And draw it
                            _spriteBatch.Draw(MineralTexture(mineral.Type), mineral.Position - _camera.Position, Color.White);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a flora.
        /// </summary>
        /// <param name="floraType">The flora type.</param>
        /// <param name="tile">The tile where the flora will be positioned on.</param>
        public void AddFlora(FloraType floraType, Tile tile)
        {
            // If the tile already hasn't an object and if the flora object doesn't already exists
            if (!tile.IsOccupied && !FloraObjects.ContainsKey(tile.Position))
            {
                // Create a new flora object and set it to the tile's position and pass the flora type
                Flora flora = new Flora(tile.Position, floraType);

                // Tile is occupied now
                tile.IsOccupied = true;
                // Pass the flora object to the tile
                tile.Flora = flora;

                // Add the flora object to the dictionary 
                FloraObjects.Add(tile.Position, flora);                   
            }
        }

        /// <summary>
        /// Adds a mineral.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tile">The tile where the mineral will be positioned on.</param>
        public void AddMineral(MineralType type, Tile tile)
        {
            // If the tile already hasn't an object and if the mineral object doesn't already exists
            if (!tile.IsOccupied && !MineralObjects.ContainsKey(tile.Position))
            {
                // Create a new mineral object and set it to the tile's position and pass the mineral type
                Mineral mineral = new Mineral(tile.Position, type);

                // Tile is occupied now
                tile.IsOccupied = true;
                // Pass the mineral object to the tile
                tile.Mineral = mineral;

                // Add the mineral object to the dictionary 
                MineralObjects.Add(tile.Position, mineral);
            }
        }

        /// <summary>
        /// Removes a flora.
        /// </summary>
        /// <param name="flora">The flora.</param>
        /// <param name="tile">The tile.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveFlora(Flora flora, Tile tile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a mineral.
        /// </summary>
        /// <param name="mineral">The mineral.</param>
        /// <param name="tile">The tile.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveMineral(Mineral mineral, Tile tile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the texture from the flora type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        // TODO: REMOVE, a texture atlas will be added later
        private Texture2D FloraTexture(FloraType type)
        {
            // Get the corresponding texture
            switch (type)
            {
                case FloraType.OakTree: return _assetManager.MapleTree;
                case FloraType.PoplarTree: return _assetManager.OakTree;
                case FloraType.RasberryBush: return _assetManager.Bush;
                default: return null;
            }
        }

        /// <summary>
        /// Gets the texture from the mineral type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        // TODO: REMOVE, a texture atlas will be added later
        private Texture2D MineralTexture(MineralType type)
        {
            // Get the corresponding texture
            switch (type)
            {
                case MineralType.CoalOre: return _assetManager.CoalOre;
                case MineralType.IronOre: return _assetManager.IronOre;
                case MineralType.StoneOre: return _assetManager.StoneOre;
                case MineralType.None: return _assetManager.Cave;
                default: return null;
            }
        }
    }
}
