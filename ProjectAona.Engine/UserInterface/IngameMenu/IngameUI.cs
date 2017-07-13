using Microsoft.Xna.Framework;
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

namespace ProjectAona.Engine.UserInterface.IngameMenu
{
    /// <summary>
    /// The playing state interface.
    /// </summary>
    public class IngameUI
    {
        protected static List<MenuButton> _menuButtons;

        //private static List<MenuButton> _subMenuButtons;

        protected Game _game;

        protected AssetManager _assetManager;

        protected SpriteBatch _spriteBatch;

        protected static bool _showSubMenu;

        protected TextureAtlas _menuItems;

        #region Menu item names for texture atlas

        protected string _menu = "menuButton";
        protected string _subMenu = "subMenuButton";
        protected string _task = "taskButton";
        protected string _explanation = "explanationBox";

        #endregion

        public delegate void ElementClicked(string element, MouseState mouseState);
        public event ElementClicked MenuClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="IngameUI"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="assetManager">The asset manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public IngameUI(Game game, AssetManager assetManager, SpriteBatch spriteBatch)
        {
            // Setters
            _menuButtons = new List<MenuButton>();
            _game = game;
            _assetManager = assetManager;
            _spriteBatch = spriteBatch;
            _showSubMenu = false;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize()
        {
            _menuItems = new TextureAtlas("menuItems", _assetManager.MenuItems, _assetManager.MenuItemsTextureAtlasXML);

            //--------------- Main menu buttons ---------------\\
            MenuButton buildButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.BUILD, _spriteBatch);
            MenuButton agricultureButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.AGRICULTURE, _spriteBatch);
            MenuButton jobsButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.JOBS, _spriteBatch);
            MenuButton researchButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.RESEARCH, _spriteBatch);
            MenuButton relationsButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.RELATIONS, _spriteBatch);

            MenuButton removeButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.DECONSTRUCT, _spriteBatch); // TODO: REMOVE

            // Add the buttons
            _menuButtons.Add(buildButton);
            _menuButtons.Add(agricultureButton);
            _menuButtons.Add(jobsButton);
            _menuButtons.Add(researchButton);
            _menuButtons.Add(relationsButton);

            _menuButtons.Add(removeButton);  // TODO: Remove

            // Devide the width of the screen by the total number of main menu buttons. So they will spread evenly
            float textureWidth = _game.GraphicsDevice.Viewport.Width / _menuButtons.Count;
            // By subtracting the button height from the height of the screen, you place them down at the bottom of the screen
            int heightIndex = _game.GraphicsDevice.Viewport.Height - _menuItems[_menu].Height;
            // Create a width index set to 0. Used to positon the main menu buttons next to each other 
            float widthIndex = 0;

            // For each main menu button
            foreach (MenuButton button in _menuButtons)
            {
                // Set the button position as a rectangle with the start position as widthIndex/heightIndex and the size of the previous calculated width, 
                // and the height in pixels from the texture
                button.Position = new Rectangle((int)widthIndex, heightIndex, (int)textureWidth, _menuItems[_menu].Height);
                // Subscribe to the OnMenuClick event
                button.ClickEvent += OnMenuClick;

                // Add the previous calculated texture width to the width index
                widthIndex += textureWidth;
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
            if (element == GameText.BuildMenu.BUILD)
            {
                // If sub menu is shown
                if (_showSubMenu)
                    _showSubMenu = false;
                else
                    _showSubMenu = true;
            }

            MenuClicked(element, mouseState);
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (MenuButton button in _menuButtons)
            {
                button.Update();
            }
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            foreach (MenuButton button in _menuButtons)
            {
                button.Draw();
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

            foreach (MenuButton button in _menuButtons)
            {
                Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                // If mouse is intersecting with the button
                if (button.Position.Contains(mousePosition))
                    return true;
            }

            return false;
        }

        public static void SelectedTileInfo(SelectionInfo selection)
        {
            Debug.WriteLine(selection.Tile.Position);
            if (selection.Entities.Count != 0)
                foreach (ISelectableInterface entity in selection.Entities)
                    Debug.WriteLine(entity.GetName());
        } 

        public static bool ShowSubMenu()
        {
            return _showSubMenu;
        }

        public static int MenuItemsCount()
        {
            return _menuButtons.Count;
        }
    }
}
