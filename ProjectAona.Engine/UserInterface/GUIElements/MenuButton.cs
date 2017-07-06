using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectAona.Test.UserInterface.GUIElements
{
    /// <summary>
    /// The menu button.
    /// </summary>
    public class MenuButton
    {
        private Texture2D _texture;

        private SpriteFont _font;

        private Rectangle _position;

        private string _menuText;

        private SpriteBatch _spriteBatch;

        public delegate void ElementClicked(string element, MouseState mouseState);
        public event ElementClicked OnClickEvent;

        private MouseState _previousMouseState;

        public Rectangle Position { get { return _position; } set { _position = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="font">The font.</param>
        /// <param name="menuText">The menu text.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public MenuButton(Texture2D texture, SpriteFont font, string menuText, SpriteBatch spriteBatch)
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
                OnClickEvent(_menuText, _previousMouseState);
            }

            _previousMouseState = currentMouseState;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            _spriteBatch.Draw(_texture, _position, Color.White);
            _spriteBatch.DrawString(_font, _menuText, new Vector2(_position.X + 20, _position.Y + 10), Color.White);
        }
    }
}
