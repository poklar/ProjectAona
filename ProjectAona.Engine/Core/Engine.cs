using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Core.Config;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Input;
using ProjectAona.Engine.Menu;
using ProjectAona.Engine.UserInterface;
using ProjectAona.Engine.World;
using System;

namespace ProjectAona.Engine.Core
{
    public class Engine
    {
        /// <summary>
        /// Gets the engine configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public EngineConfig Configuration { get; private set; }

        /// <summary>
        /// Gets the attached game.
        /// </summary>
        /// <value>
        /// The attached game.
        /// </value>
        public Game Game { get; private set; }

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        public delegate void EngineStartHandler(object sender, EventArgs e);
        public event EngineStartHandler EngineStart;

        public Engine(Game game, EngineConfig config, SpriteBatch spriteBatch)
        {
            // Check if instance isn't set already
            if (_instance != null)
                throw new Exception("You can not instantiate the Engine more than once");

            // Set the instance
            _instance = this;
            
            Game = game;
            Configuration = config;
            _spriteBatch = spriteBatch;

            // Validate the config
            config.Validate();
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            Initialize();
            NotifyEngineStart(EventArgs.Empty);
        }

        /// <summary>
        /// Notifies the engine start.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void NotifyEngineStart(EventArgs e)
        {
            EngineStartHandler handler = EngineStart;
            if (handler != null)
                handler(typeof(Engine), e);
        }

        private AssetManager _assetManager;
        private ChunkManager _chunkManager;
        private Camera _camera;
        private TerrainManager _terrainManager;
        private PlayingStateInterface _playingStateInterface;
        private BuildMenuManager _buildMenuManager;
        private NPCManager _npcManager;
        private MouseManager _mouseManager;
        
        public Camera Camera { get { return _camera; } }

        /// <summary>
        /// Adds the components.
        /// </summary>
        public void Initialize()
        {
            _assetManager = new AssetManager(Game);
            _assetManager.Initialize();

            _camera = new Camera(Game);

            _chunkManager = new ChunkManager(Game, _spriteBatch, _camera, _assetManager);
            _chunkManager.Initialize();

            _mouseManager = new MouseManager(_camera, _chunkManager);

            _playingStateInterface = new PlayingStateInterface(Game, _assetManager, _spriteBatch);
            _playingStateInterface.Initialize();

            _terrainManager = new TerrainManager(_spriteBatch, _camera, _assetManager, _chunkManager);

            _buildMenuManager = new BuildMenuManager(_spriteBatch, _camera, _chunkManager, _playingStateInterface, _assetManager, _terrainManager);

            _npcManager = new NPCManager(_camera, _assetManager, _chunkManager, _spriteBatch);
            _npcManager.Initialize();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            _terrainManager.LoadContent();
            
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);
            _buildMenuManager.Update(gameTime);
            _npcManager.Update(gameTime);
            _playingStateInterface.Update(gameTime);
            _mouseManager.Update(gameTime);
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _chunkManager.Draw(gameTime);
            _terrainManager.Draw(gameTime);
            _buildMenuManager.Draw(gameTime);
            _npcManager.Draw(gameTime);
            _playingStateInterface.Draw(gameTime);
        }


        /// <summary>
        /// The memory instance.
        /// </summary>
        private static Engine _instance;

        public static Engine Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Is the engine instance disposed already?
        /// </summary>
        public bool Disposed = false;

        private void Dispose(bool disposing)
        {
            // If disposed already
            if (Disposed)
                return;

            // Only dispose managed resources if we're called from directly or in-directly from user code
            if (disposing)
                _instance = null;

            Disposed = true;
        }

        public void Dispose()
        {
            // Object being disposed by the code itself, dispose both managed and unmanaged objects
            Dispose(true);
            // Take object out the finalization queue to prevent finalization code for it from executing a second time
            GC.SuppressFinalize(this); 
        }

        // finalizer called by the runtime. we should only dispose unmanaged objects and should NOT reference managed ones
        ~Engine() { Dispose(false); } 
    }
}
