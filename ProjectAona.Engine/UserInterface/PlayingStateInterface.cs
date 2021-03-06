﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Test.UserInterface.GUIElements;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Common;
using ProjectAona.Engine.Input;
using System.Diagnostics;
using ProjectAona.Engine.World.Selection;
using MonoGame.Extended.TextureAtlases;

namespace ProjectAona.Engine.UserInterface
{
    /// <summary>
    /// The playing state interface.
    /// </summary>
    public class PlayingStateInterface
    {
        private static List<MenuButton> _menuButtons;

        private static List<MenuButton> _subMenuButtons;

        private Game _game;

        private AssetManager _assetManager;

        private SpriteBatch _spriteBatch;

        static private bool _showSubMenu;

        private TextureAtlas _menuItems;

        #region Menu item names for texture atlas

        private string _menu = "menuButton";
        protected string _subMenu = "subMenuButton";
        protected string _task = "taskButton";
        protected string _explanation = "explanationBox";

        #endregion

        public delegate void ElementClicked(string element, MouseState mouseState);
        public event ElementClicked OnSubMenuClicked;
        public event ElementClicked OnMenuClicked;

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
            _menuItems = new TextureAtlas("menuItems", _assetManager.MenuItems, _assetManager.MenuItemsTextureAtlasXML);

            //--------------- Main menu buttons ---------------\\
            MenuButton buildButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.BUILDWALL, _spriteBatch);
            MenuButton testButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.REMOVE, _spriteBatch);

            // Add the buttons
            _menuButtons.Add(buildButton);
            _menuButtons.Add(testButton);  

            // Devide the width of the screen by the total number of main menu buttons. So they will spread evenly
            float textureWidth = _game.GraphicsDevice.Viewport.Width / _menuButtons.Count;
            // By subtracting the button height from the height of the screen, you place them down at the bottom of the screen
            int heightIndex = _game.GraphicsDevice.Viewport.Height - _menuItems[_menu].Height;
            // Create a width index set to 0. Used to positon the main menu buttons next to each other 
            int widthIndex = 0;

            // For each main menu button
            foreach (MenuButton button in _menuButtons)
            {
                // Set the button position as a rectangle with the start position as widthIndex/heightIndex and the size of the previous calculated width, 
                // and the height in pixels from the texture
                button.Position = new Rectangle(widthIndex, heightIndex, (int)textureWidth, _menuItems[_menu].Height);
                // Subscribe to the OnMenuClick event
                button.OnClickEvent += OnMenuClick;

                // Add the previous calculated texture width to the width index
                widthIndex += (int)textureWidth;
            }

            //--------------- Sub menu buttons ---------------\\
            MenuButton buildWoodWallButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.BUILDWOODWALL, _spriteBatch);
            MenuButton buildBrickWallButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.BUILDBRICKWALL, _spriteBatch);
            MenuButton buildStoneWallButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.BUILDSTONEWALL, _spriteBatch);

            // Add the buttons
            _subMenuButtons.Add(buildWoodWallButton);
            _subMenuButtons.Add(buildBrickWallButton);
            _subMenuButtons.Add(buildStoneWallButton);

            // Devide the width of the screen by the total number of main menu buttons. So they will spread evenly
            textureWidth = _game.GraphicsDevice.Viewport.Width / _menuButtons.Count;
            // The screen height is subtracted by the menu buttons height times the total of buttons plus 1. The plus one stands for the main menu button
            heightIndex = _game.GraphicsDevice.Viewport.Height - (_menuItems[_menu].Height * (_subMenuButtons.Count + 1));
            // Create a width index set to 0
            widthIndex = 0;

            // For each sub menu button
            foreach (MenuButton button in _subMenuButtons)
            {
                // Set the button position as a rectangle with the start position as widthIndex/heightIndex and the size of the width, 
                // and the height in pixels from the texture
                button.Position = new Rectangle(widthIndex, heightIndex, (int)textureWidth, _menuItems[_menu].Height);
                // Subscribe to the event OnSubMenuClick
                button.OnClickEvent += OnSubMenuClick;

                // Add the height in pixels to the previous calculated height index
                heightIndex += _menuItems[_menu].Height;
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
                    _showSubMenu = false;
                else
                    _showSubMenu = true;
            }
            else if (element == GameText.BuildMenu.REMOVE)
                OnMenuClicked(element, mouseState);

        }

        /// <summary>
        /// Called when [sub menu click].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        private void OnSubMenuClick(string element, MouseState mouseState)
        {
            if (element == GameText.BuildMenu.BUILDWOODWALL)
                OnSubMenuClicked(element, mouseState);
            else if (element == GameText.BuildMenu.BUILDBRICKWALL)
                OnSubMenuClicked(element, mouseState);
            else if (element == GameText.BuildMenu.BUILDSTONEWALL)
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
        public static bool IsMouseOverMenu()
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

            if (_showSubMenu)
            {
                foreach (MenuButton button in _subMenuButtons)
                {
                    Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                    // If mouse is intersecting with the button
                    if (button.Position.Contains(mousePosition))
                        return true;
                }
            }

            return mouseOverMenu;
        }

        public static void SelectedTileInfo(SelectionInfo selection)
        {
            Debug.WriteLine(selection.Tile.Position);
            if (selection.Entities.Count != 0)
                foreach (ISelectableInterface entity in selection.Entities)
                    Debug.WriteLine(entity.GetName());
        } 
    }
}
