using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Common;
using ProjectAona.Test.UserInterface.GUIElements;

namespace ProjectAona.Engine.UserInterface.IngameMenu.BuildMenu
{
    public class BuildMenuUI
    {
        private Game _game;

        private AssetManager _assetManager;

        private SpriteBatch _spriteBatch;
        
        private static MenuButton[] _subMenuButtons;

        public delegate void ElementClicked(string element, MouseState mouseState);
        public event ElementClicked SubMenuClicked;

        protected TextureAtlas _menuItems;

        #region Menu item names for texture atlas

        protected string _menu = "menuButton";
        protected string _subMenu = "subMenuButton";
        protected string _task = "taskButton";
        protected string _explanation = "explanationBox";

        #endregion

        public BuildMenuUI(Game game, AssetManager assetManager, SpriteBatch spriteBatch)
        {
            _game = game;
            _assetManager = assetManager;
            _spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
            _menuItems = new TextureAtlas("menuItems", _assetManager.MenuItems, _assetManager.MenuItemsTextureAtlasXML);

            MenuButton workshopButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.WORKSHOP, _spriteBatch);
            MenuButton structureButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.STRUCTURE, _spriteBatch);
            MenuButton furnitureButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.FURNITURE, _spriteBatch);
            MenuButton storageButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.STORAGE, _spriteBatch);
            MenuButton floorButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.FLOOR, _spriteBatch);
            MenuButton roomButton = new MenuButton(_menuItems[_menu], _assetManager.InGameFont, GameText.BuildMenu.ROOM, _spriteBatch);

            _subMenuButtons = new MenuButton[6];

            _subMenuButtons[0] = workshopButton;
            _subMenuButtons[1] = structureButton;
            _subMenuButtons[2] = furnitureButton;
            _subMenuButtons[3] = storageButton;
            _subMenuButtons[4] = floorButton;
            _subMenuButtons[5] = roomButton;

            // Devide the width of the screen by the total number of main menu buttons. So they will spread evenly
            float textureWidth = _game.GraphicsDevice.Viewport.Width / _subMenuButtons.GetLength(0);
            // The screen height is subtracted by the menu buttons height times the total of buttons plus 1. The plus one stands for the main menu button
            int heightIndex = _game.GraphicsDevice.Viewport.Height - (_menuItems[_menu].Height * (_subMenuButtons.GetLength(0) + 1));
            // Create a width index set to 0
            int widthIndex = 0;

            for (int i = 0; i < _subMenuButtons.GetLength(0); i++)
            {
                // Set the button position as a rectangle with the start position as widthIndex/heightIndex and the size of the width, 
                // and the height in pixels from the texture
                _subMenuButtons[i].Position = new Rectangle(widthIndex, heightIndex, (int)textureWidth, _menuItems[_menu].Height);
                // Subscribe to the event OnSubMenuClick
                _subMenuButtons[i].ClickEvent += OnSubMenuClick;

                // Add the height in pixels to the previous calculated height index
                heightIndex += _menuItems[_menu].Height;
            }
        }

        public void Update(GameTime gameTime)
        {
            // If the sub many is displaying
            if (IngameUI.ShowSubMenu())
            {
                for (int i = 0; i < _subMenuButtons.GetLength(0); i++)
                    _subMenuButtons[i].Update();
            }
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            // If the sub many is displaying
            if (IngameUI.ShowSubMenu())
            {
                for (int i = 0; i < _subMenuButtons.GetLength(0); i++)
                    _subMenuButtons[i].Draw();
            }

            _spriteBatch.End();
        }

        /// <summary>
        /// Called when [sub menu click].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        private void OnSubMenuClick(string element, MouseState mouseState)
        {
            SubMenuClicked(element, mouseState);
        }

        public static bool IsMouseOverMenu()
        {
            if (IngameUI.ShowSubMenu())
            {
                MouseState currentMouseState = Mouse.GetState();

                for (int i = 0; i < _subMenuButtons.GetLength(0); i++)
                {
                    Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                    // If mouse is intersecting with the button
                    if (_subMenuButtons[i].Position.Contains(mousePosition))
                        return true;
                }
            }

            return false;
        }

    }
}
