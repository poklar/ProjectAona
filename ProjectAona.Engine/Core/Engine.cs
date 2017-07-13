using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Core.Config;
using ProjectAona.Engine.Debugging;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Input;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Pathfinding;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.UserInterface;
using ProjectAona.Engine.UserInterface.IngameMenu;
using ProjectAona.Engine.UserInterface.IngameMenu.BuildMenu;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Items.Resources;
using System;
using System.Collections.Generic;

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
        private NPCManager _npcManager;
        private MouseManager _mouseManager;
        private JobManager _jobManager;
        private FrameRateCounter _frameRateCounter;
        private StructureManager _structureManager;
        private StorageManager _storageManager;
        private ItemManager _itemManager;

        private IngameUI _ingameUI;
        private BuildMenuUI _buildMenuUI;
        private StructureUI _structureUI;
        private StorageUI _storageUI;

        private static Graph _graph;
        
        public Camera Camera { get { return _camera; } }

        public static Graph Graph { get { return _graph; } set { _graph = value; } }

        /// <summary>
        /// Adds the components.
        /// </summary>
        public void Initialize()
        {
            _assetManager = new AssetManager(Game);
            _assetManager.Initialize();

            _frameRateCounter = new FrameRateCounter(_assetManager, _spriteBatch);

            _camera = new Camera(Game);

            _chunkManager = new ChunkManager(Game, _spriteBatch, _camera, _assetManager);
            

            _mouseManager = new MouseManager(_camera);

            _ingameUI = new IngameUI(Game, _assetManager, _spriteBatch);
            _ingameUI.Initialize();

            _buildMenuUI = new BuildMenuUI(Game, _assetManager, _spriteBatch);
            _buildMenuUI.Initialize();
            
            _structureUI = new StructureUI(Game, _assetManager, _spriteBatch, _buildMenuUI, _ingameUI);
            _structureUI.Initialize();

            _storageUI = new StorageUI(Game, _assetManager, _spriteBatch, _buildMenuUI, _ingameUI);
            _storageUI.Initialize();

            _terrainManager = new TerrainManager(_spriteBatch, _camera, _assetManager);
            _chunkManager.Initialize();

            _jobManager = new JobManager(_terrainManager);

            _structureManager = new StructureManager(_spriteBatch, _camera, _structureUI, _assetManager, _jobManager);
            _storageManager = new StorageManager(_spriteBatch, _camera, _assetManager, _storageUI);

            _npcManager = new NPCManager(_camera, _assetManager, _spriteBatch);
            _npcManager.Initialize();

            _itemManager = new ItemManager(_jobManager);

            _graph = new Graph();

            StockpileManager stockpileManager = new StockpileManager();
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
            _structureManager.Update(gameTime);
            _storageManager.Update(gameTime);
            _npcManager.Update(gameTime);

            _ingameUI.Update(gameTime);
            _buildMenuUI.Update(gameTime);
            _structureUI.Update(gameTime);
            _storageUI.Update(gameTime);
            
            _mouseManager.Update(gameTime);
            _frameRateCounter.Update(gameTime);

            

        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _chunkManager.Draw(gameTime);
            _terrainManager.Draw(gameTime);
            _structureManager.Draw(gameTime);
            _storageManager.Draw(gameTime);
            _npcManager.Draw(gameTime);



            _ingameUI.Draw(gameTime);
            _buildMenuUI.Draw(gameTime);
            _structureUI.Draw(gameTime);
            _storageUI.Draw(gameTime);

            _frameRateCounter.Draw(gameTime);
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
