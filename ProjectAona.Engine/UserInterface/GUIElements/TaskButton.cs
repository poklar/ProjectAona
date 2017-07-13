using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;

namespace ProjectAona.Test.UserInterface.GUIElements
{
    /// <summary>
    /// The task button.
    /// </summary>
    public class TaskButton
    {
        private TextureRegion2D _texture;

        private SpriteFont _font;

        private Rectangle _position;

        private string _menuText;

        private SpriteBatch _spriteBatch;

        public delegate void ElementClicked(string element, MouseState mouseState);
        public event ElementClicked ClickEvent;

        private MouseState _previousMouseState;

        public Rectangle Position { get { return _position; } set { _position = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskButton"/> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="font">The font.</param>
        /// <param name="menuText">The menu text.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public TaskButton(TextureRegion2D texture, SpriteFont font, string menuText, SpriteBatch spriteBatch)
        {
            _texture = texture;
            _font = font;
            _position = new Rectangle();
            _menuText = menuText;
            _spriteBatch = spriteBatch;

            _previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState(); ;

            if (_position.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                ClickEvent(_menuText, _previousMouseState);
            }

            _previousMouseState = currentMouseState;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            Vector2 textPosition = new Vector2(_position.X, _position.Y + _texture.Bounds.Height);

            _spriteBatch.Draw(_texture.Texture, _position, _texture.Bounds, Color.White);
            _spriteBatch.DrawString(_font, _menuText, textPosition, Color.White);
        }
    }
}
