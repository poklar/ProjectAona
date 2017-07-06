using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.Graphics
{
    /// <summary>
    /// The camera.
    /// </summary>
    public class Camera
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
        /// The viewport.
        /// </summary>
        private Rectangle _viewport;
        /// <summary>
        /// The zoom.
        /// </summary>
        private float _zoom;

        /// <summary>
        /// The game.
        /// </summary>
        private Game _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public Camera(Game game)
        {
            _game = game;
            Zoom = 1.0f;
            Position = new Vector2(400f, 300);
            _viewport = _game.GraphicsDevice.Viewport.Bounds;

            CalculateView();
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
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Moves the camera.
        /// </summary>
        /// <param name="position">The position.</param>
        public void MoveCamera(Vector2 position)
        {
            // Change position
            Position = position;
            CalculateView();
        }

        /// <summary>
        /// Updates the zoom.
        /// </summary>
        /// <param name="zoom">The zoom.</param>
        public void UpdateZoom(float zoom)
        {
            _zoom = zoom;
            CalculateView();
        }

        private void CalculateView()
        {
            // Calculate view
            View = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(new Vector3(_viewport.Width / 2.0f, _viewport.Height / 2.0f, 0));

            // Get the top left world position of the screen
            Vector2 viewportWorldPosition = Vector2.Transform(new Vector2(0, 0), Matrix.Invert(View));

            //  Set the screen
            ScreenRectangle = new Rectangle((int)viewportWorldPosition.X, (int)viewportWorldPosition.Y, (int)(_viewport.Width / Zoom), (int)(_viewport.Height / Zoom));
        }
    }
}
