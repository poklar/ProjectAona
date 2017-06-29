using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Chunk.Generators;
using ProjectAona.Engine.Core.Config;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Input;
using ProjectAona.Engine.Tiles;
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

        public Engine(Game game, EngineConfig config)
        {
            // Check if instance isn't set already
            if (_instance != null)
                throw new Exception("You can not instantiate the Egine more than once");

            // Set the instance
            _instance = this;
            
            Game = game;
            Configuration = config;
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Validate the config
            config.Validate();
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            AddComponents();
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

        /// <summary>
        /// Adds the components.
        /// </summary>
        private void AddComponents()
        {
            Game.Components.Add(new InputManager(Game));
            Game.Components.Add(new AssetManager(Game));
            Game.Components.Add(new ChunkManager(Game, _spriteBatch));
            Game.Components.Add(new SimpleTerrain(Game));
            Game.Components.Add(new TerrainManager(Game, _spriteBatch));
            Game.Components.Add(new TileTexture(Game));
            Game.Components.Add(new Camera(Game));
            Game.Components.Add(new ChunkStorage(Game));
            Game.Components.Add(new ChunkCache(Game));
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
