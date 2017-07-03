using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.Core
{
    /// <summary>
    /// The game loop.
    /// </summary>
    public abstract class GameLoop
    {
        /// <summary>
        /// The game.
        /// </summary>
        public Game _game;

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public Game Game { get { return _game; } set { _game = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLoop"/> class.
        /// </summary>
        public GameLoop()
        {

        }        

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Loads the content.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public abstract void Draw(GameTime gameTime);
    }
}
