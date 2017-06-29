using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.Graphics
{
    /// <summary>
    /// The camera.
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// Gets the zoom.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        float Zoom { get; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        Matrix View { get; }

        /// <summary>
        /// Gets the screen rectangle (view).
        /// </summary>
        /// <value>
        /// The screen rectangle.
        /// </value>
        Rectangle ScreenRectangle { get; }
    }

    /// <summary>
    /// The camera controller.
    /// </summary>
    public interface ICameraController
    {
        /// <summary>
        /// Moves the camera.
        /// </summary>
        /// <param name="position">The position.</param>
        void MoveCamera(Vector2 position);

        /// <summary>
        /// Updates the zoom.
        /// </summary>
        /// <param name="zoom">The zoom.</param>
        void UpdateZoom(float zoom);
    }

    /// <summary>
    /// The camera.
    /// </summary>
    /// <seealso cref="Microsoft.Xna.Framework.GameComponent" />
    /// <seealso cref="ProjectAona.Engine.Graphics.ICamera" />
    /// <seealso cref="ProjectAona.Engine.Graphics.ICameraController" />
    public class Camera : GameComponent, ICamera, ICameraController
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Gets the screen rectangle (view).
        /// </summary>
        /// <value>
        /// The screen rectangle.
        /// </value>
        public Rectangle ScreenRectangle { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public Matrix View { get; private set; }

        /// <summary>
        /// The view port.
        /// </summary>
        private Rectangle viewPort;
        /// <summary>
        /// The zoom.
        /// </summary>
        private float _zoom;

        private Rectangle _bounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public Camera(Game game)
            : base(game)
        {
            // Export services
            game.Services.AddService(typeof(ICamera), this);
            game.Services.AddService(typeof(ICameraController), this);

            // Setters
            Zoom = 1.0f;
            Position = Vector2.Zero;
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.5f) _zoom = 0.5f; } // Negative zoom will flip image
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Get the viewport boundary
            viewPort = Game.GraphicsDevice.Viewport.Bounds;
            _bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)(viewPort.Width / Zoom), (int)(viewPort.Height / Zoom));

            // Change viewport's position
            viewPort.Offset((int)Position.X, (int)Position.Y);
            // Set viewport to screen rectangle
            ScreenRectangle = _bounds;

            View = //Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateScale(Zoom, Zoom, 1.0f);
                   Matrix.CreateTranslation(viewPort.Width / 2, viewPort.Height / 2, 0.0f);


            base.Update(gameTime);
        }

        /// <summary>
        /// Moves the camera.
        /// </summary>
        /// <param name="position">The position.</param>
        public void MoveCamera(Vector2 position)
        {
            // Change position
            Position = position;
        }

        /// <summary>
        /// Updates the zoom.
        /// </summary>
        /// <param name="zoom">The zoom.</param>
        public void UpdateZoom(float zoom)
        {
            _zoom = zoom;
        }
    }
}
