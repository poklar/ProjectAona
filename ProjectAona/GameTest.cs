using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Chunk.Generators;
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
        private SpriteBatch _spriteBatch;
        private Vector2 _cameraPosition, _previousMousePosition;
        private ChunkStorage _chunkStorage;
        private Rectangle _screenRectangle;

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
            // Set the window title
            Window.Title = "Project Aona Test";
           
            EngineConfig config = new EngineConfig();

            var engine = new Engine.Core.Engine(this, config);
            GraphicsManager = new GraphicsManager(_graphicsDeviceManager, this);

            engine.EngineStart += OnEngineStart;

            engine.Run();

            base.Initialize();
        }

        private void OnEngineStart(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            IsMouseVisible = true;

            // TODO: TEMPORARY, REMOVE LATER (don't forget to delete the content)
            StaticData.StoneTexture = Content.Load<Texture2D>("Textures\\stoneTex");
            StaticData.DefaultFont = Content.Load<SpriteFont>("Fonts\\DefaultFont");

            ITestTerrain<Chunk> generator = new TestTerrain(Engine.Core.Engine.Instance.Configuration.Chunk.WidthInTiles,
                                                            Engine.Core.Engine.Instance.Configuration.Chunk.HeightInTiles,
                                                            32);
            _chunkStorage = new ChunkStorage(generator);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _screenRectangle = GraphicsDevice.Viewport.Bounds;
            _screenRectangle.Offset((int)_cameraPosition.X, (int)_cameraPosition.Y);

            ReactToKeyboardInput();

            base.Update(gameTime);
        }

        private void ReactToKeyboardInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float moveSpeed = 3;
            if (keyboardState.IsKeyDown(Keys.Up)) { _cameraPosition -= Vector2.UnitY * moveSpeed; }
            if (keyboardState.IsKeyDown(Keys.Down)) { _cameraPosition += Vector2.UnitY * moveSpeed; }
            if (keyboardState.IsKeyDown(Keys.Left)) { _cameraPosition -= Vector2.UnitX * moveSpeed; }
            if (keyboardState.IsKeyDown(Keys.Right)) { _cameraPosition += Vector2.UnitX * moveSpeed; }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            DrawAllPartiallyVisibleChunks();
            DrawCurrentlyLoadedChunksInfo();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawAllPartiallyVisibleChunks()
        {
            foreach (Chunk chunk in _chunkStorage.GetVisibleChunks(_screenRectangle))
            {
                chunk.Draw(_spriteBatch, _cameraPosition);
            }
        }

        private void DrawCurrentlyLoadedChunksInfo()
        {
            Vector2 textOffset = Vector2.UnitY * 20;
            Vector2 textPosition = Vector2.UnitY * 260;
            _spriteBatch.DrawString(StaticData.DefaultFont, "Currently loaded worldQuadrants", textPosition, Color.White);

            textPosition += textOffset;

            foreach (var chunk in _chunkStorage.QuadrantsCurrentlyInMemory)
            {
                _spriteBatch.DrawString(StaticData.DefaultFont, chunk.ToString(), textPosition, Color.White);
                textPosition += textOffset;
            }

        }
    }
}
