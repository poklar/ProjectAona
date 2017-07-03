using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Graphics;

namespace ProjectAona.Test
{
    public interface IPlayer
    {

    }

    public class Player : GameComponent, IPlayer
    {
        /// <summary>
        /// The camera controller.
        /// </summary>
        private ICameraController _cameraController;

        /// <summary>
        /// The camera.
        /// </summary>
        private ICamera _camera;

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        private MouseState _previousMouseState;

        /// <summary>
        /// The previous scroll value
        /// </summary>
        private int _previousScrollValue;

        /// <summary>
        /// The previous keyboard state.
        /// </summary>
        private KeyboardState _previousKeyboardState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public Player(Game game)
            : base(game)
        {
            // Export service.
            Game.Services.AddService(typeof(IPlayer), this);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            // Get services
            _cameraController = (ICameraController)Game.Services.GetService(typeof(ICameraController));
            _camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            // Setters
            _previousMouseState = Mouse.GetState();
            _previousScrollValue = _previousMouseState.ScrollWheelValue;
            _previousKeyboardState = Keyboard.GetState();

            int i = 72;

            i -= i % 32;

        }

        public override void Update(GameTime gameTime)
        {
            ProcessKeyboard(gameTime);
            ProcessMouse();

            // TODO: Add state if buildmode
            OnSelection();

            base.Update(gameTime);
        }

        private void OnSelection()
        {
            MouseState mouseState = Mouse.GetState();

            int positionX = mouseState.X;
            int positionY = mouseState.Y;

            Tile tile = 


        }

        /// <summary>
        /// Processes the keyboard.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        private void ProcessKeyboard(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            // Allows quick exiting of the game.
            if (currentState.IsKeyDown(Keys.Escape))
                Game.Exit();

            // TODO: Get this from a config file (player/world)
            float moveSpeed = 3;

            // Get the camera position
            Vector2 position = _camera.Position;

            if (currentState.IsKeyDown(Keys.Up))
            {
                // Calculate position and move camera
                position -= Vector2.UnitY * moveSpeed;
                _cameraController.MoveCamera(position);
            }
            if (currentState.IsKeyDown(Keys.Down))
            {
                // Calculate position and move camera
                position += Vector2.UnitY * moveSpeed;
                _cameraController.MoveCamera(position);
            }
            if (currentState.IsKeyDown(Keys.Left))
            {
                // Calculate position and move camera
                position -= Vector2.UnitX * moveSpeed;
                _cameraController.MoveCamera(position);
            }
            if (currentState.IsKeyDown(Keys.Right))
            {
                // Calculate position and move camera
                position += Vector2.UnitX * moveSpeed;
                _cameraController.MoveCamera(position);
            }
        }

        private void ProcessMouse()
        {
            MouseState currentMouseState = Mouse.GetState();

            // TODO: Get this from a config file (player/world)
            float scale = 0.025f;

            float zoom = _camera.Zoom;

            if (currentMouseState.ScrollWheelValue < _previousScrollValue)
            {
                zoom -= scale;
                _cameraController.UpdateZoom(zoom);
            }
            else if (currentMouseState.ScrollWheelValue > _previousScrollValue)
            {
                zoom += scale;
                _cameraController.UpdateZoom(zoom);
            }

            _previousScrollValue = currentMouseState.ScrollWheelValue;
        }
    }
}
