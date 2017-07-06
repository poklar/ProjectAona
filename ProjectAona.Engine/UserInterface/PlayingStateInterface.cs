using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Test.UserInterface.GUIElements;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Common;

namespace ProjectAona.Engine.UserInterface
{
    /// <summary>
    /// The playing state interface.
    /// </summary>
    public class PlayingStateInterface
    {
        /// <summary>
        /// The menu buttons.
        /// </summary>
        private List<MenuButton> _menuButtons;
        /// <summary>
        /// The sub menu buttons.
        /// </summary>
        private List<MenuButton> _subMenuButtons;

        /// <summary>
        /// The game.
        /// </summary>
        private Game _game;

        /// <summary>
        /// The asset manager.
        /// </summary>
        private AssetManager _assetManager;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// The show sub menu.
        /// </summary>
        private bool _showSubMenu;

        /// <summary>
        /// Delegater.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        public delegate void ElementClicked(string element, MouseState mouseState);
        /// <summary>
        /// Occurs when [on sub menu clicked].
        /// </summary>
        public event ElementClicked OnSubMenuClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingStateInterface"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="assetManager">The asset manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public PlayingStateInterface(Game game, AssetManager assetManager, SpriteBatch spriteBatch)
        {
            // Setters
            _menuButtons = new List<MenuButton>();
            _subMenuButtons = new List<MenuButton>();
            _game = game;
            _assetManager = assetManager;
            _spriteBatch = spriteBatch;
            _showSubMenu = false;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            //--------------- Main menu buttons ---------------\\
            MenuButton buildButton = new MenuButton(_assetManager.MenuButton, _assetManager.InGameFont, GameText.BuildMenu.BUILDWALL, _spriteBatch);
            MenuButton testButton = new MenuButton(_assetManager.MenuButton, _assetManager.InGameFont, "Test", _spriteBatch);

            // Add the buttons
            _menuButtons.Add(buildButton);
            _menuButtons.Add(testButton);  

            // Devide the width of the screen by the total number of main menu buttons. So they will spread evenly
            float textureWidth = _game.GraphicsDevice.Viewport.Width / _menuButtons.Count;
            // By subtracting the button height from the height of the screen, you place them down at the bottom of the screen
            int heightIndex = _game.GraphicsDevice.Viewport.Height - _assetManager.MenuButton.Height;
            // Create a width index set to 0. Used to positon the main menu buttons next to each other 
            int widthIndex = 0;

            // For each main menu button
            foreach (MenuButton button in _menuButtons)
            {
                // Set the button position as a rectangle with the start position as widthIndex/heightIndex and the size of the previous calculated width, 
                // and the height in pixels from the texture
                button.Position = new Rectangle(widthIndex, heightIndex, (int)textureWidth, _assetManager.MenuButton.Height);
                // Subscribe to the OnMenuClick event
                button.OnClickEvent += OnMenuClick;

                // Add the previous calculated texture width to the width index
                widthIndex += (int)textureWidth;
            }

            //--------------- Sub menu buttons ---------------\\
            MenuButton buildWoodWallButton = new MenuButton(_assetManager.MenuButton, _assetManager.InGameFont, GameText.BuildMenu.BUILDWOODWALL, _spriteBatch);
            MenuButton buildBrickWallButton = new MenuButton(_assetManager.MenuButton, _assetManager.InGameFont, GameText.BuildMenu.BUILDBRICKWALL, _spriteBatch);
            MenuButton buildStoneWallButton = new MenuButton(_assetManager.MenuButton, _assetManager.InGameFont, GameText.BuildMenu.BUILDSTONEWALL, _spriteBatch);

            // Add the buttons
            _subMenuButtons.Add(buildWoodWallButton);
            _subMenuButtons.Add(buildBrickWallButton);
            _subMenuButtons.Add(buildStoneWallButton);

            // Devide the width of the screen by the total number of main menu buttons. So they will spread evenly
            textureWidth = _game.GraphicsDevice.Viewport.Width / _menuButtons.Count;
            // The screen height is subtracted by the menu buttons height times the total of buttons plus 1. The plus one stands for the main menu button
            heightIndex = _game.GraphicsDevice.Viewport.Height - (_assetManager.MenuButton.Height * (_subMenuButtons.Count + 1));
            // Create a width index set to 0
            widthIndex = 0;

            // For each sub menu button
            foreach (MenuButton button in _subMenuButtons)
            {
                // Set the button position as a rectangle with the start position as widthIndex/heightIndex and the size of the width, 
                // and the height in pixels from the texture
                button.Position = new Rectangle(widthIndex, heightIndex, (int)textureWidth, _assetManager.MenuButton.Height);
                // Subscribe to the event OnSubMenuClick
                button.OnClickEvent += OnSubMenuClick;

                // Add the height in pixels to the previous calculated height index
                heightIndex += _assetManager.MenuButton.Height;
            }

        }

        /// <summary>
        /// Called when [menu click].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        private void OnMenuClick(string element, MouseState mouseState)
        {
            // Main menu calls
            if (element == GameText.BuildMenu.BUILDWALL)
            {
                // If sub menu is shown
                if (_showSubMenu)
                    // Set to false
                    _showSubMenu = false;
                // Else if it isn't shown
                else
                    // Set to true
                    _showSubMenu = true;
            }
        }

        /// <summary>
        /// Called when [sub menu click].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        private void OnSubMenuClick(string element, MouseState mouseState)
        {
            // If player clicked on wood wall
            if (element == GameText.BuildMenu.BUILDWOODWALL)
                // Call event
                OnSubMenuClicked(element, mouseState);
            // If player clicked on brick wall
            else if (element == GameText.BuildMenu.BUILDBRICKWALL)
                // Call event
                OnSubMenuClicked(element, mouseState);
            // If player clicked on stone wall
            else if (element == GameText.BuildMenu.BUILDSTONEWALL)
                // Call event
                OnSubMenuClicked(element, mouseState);
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            foreach (MenuButton button in _menuButtons)
            {
                button.Update();
            }

            // If the sub many is displaying
            if (_showSubMenu)
            {
                foreach (MenuButton button in _subMenuButtons)
                {
                    button.Update();
                }
            }
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            
            foreach (MenuButton button in _menuButtons)
            {
                button.Draw();
            }

            // If the sub many is displaying
            if (_showSubMenu)
            {
                foreach (MenuButton button in _subMenuButtons)
                {
                    button.Draw();
                }
            }

            _spriteBatch.End();
        }

        /// <summary>
        /// Determines whether [is mouse over menu].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is mouse over menu]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMouseOverMenu()
        {
            MouseState currentMouseState = Mouse.GetState();

            bool mouseOverMenu = false;

            foreach (MenuButton button in _menuButtons)
            {
                Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                // If mouse is intersecting with the button
                if (button.Position.Contains(mousePosition))
                    return true;
            }

            foreach (MenuButton button in _subMenuButtons)
            {
                Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                // If mouse is intersecting with the button
                if (button.Position.Contains(mousePosition))
                    return true;
            }

            return mouseOverMenu;
        }
    }
}
