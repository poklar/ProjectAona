using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Core.Config;
using ProjectAona.Engine.Graphics;
using ProjectAona.Test.UserInterface;
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
           
            // Initialize the configurations
            EngineConfig config = new EngineConfig();

            // Create the engine
            var engine = new Engine.Core.Engine(this, config);

            // Start the screen manager
            GraphicsManager = new GraphicsManager(_graphicsDeviceManager, this);

            // Add the engine listener
            engine.EngineStart += OnEngineStart;

            // Start the engine
            engine.Run();

            Components.Add(new BuildInterface(this));
            Components.Add(new Player(this));

            base.Initialize();
        }

        private void OnEngineStart(object sender, EventArgs e)
        {
            // TODO: What to do here?
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

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
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);
        }
    }
}
