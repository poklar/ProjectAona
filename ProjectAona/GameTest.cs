﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Core.Config;
using ProjectAona.Engine.Graphics;
using System;

namespace ProjectAona.Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameTest : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        public GraphicsManager GraphicsManager { get; private set; }

        private SpriteBatch _spriteBatch;

        private Engine.Core.Engine _engine;

        private Player _player;

        public GameTest()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Set mouse visible
            IsMouseVisible = true;

            // Set the window title
            Window.Title = "Project Aona Test";

            // Spritebatch
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialize the configurations
            EngineConfig config = new EngineConfig();

            // Create the engine
            _engine = new Engine.Core.Engine(this, config, _spriteBatch);

            // Start the screen manager
            GraphicsManager = new GraphicsManager(_graphicsDeviceManager, this);

            // Add the engine listener
            _engine.EngineStart += OnEngineStart;

            // Start the engine
            _engine.Run();

            IsFixedTimeStep = false; // TODO: Remove?

            base.Initialize();
        }

        private void OnEngineStart(object sender, EventArgs e)
        {
            // TODO: What to do here?
            _player = new Player(this, _engine.Camera);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _engine.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _engine.Update(gameTime);
            _player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _engine.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
