using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Common;
using ProjectAona.Test.UserInterface.GUIElements;

namespace ProjectAona.Engine.UserInterface.IngameMenu.BuildMenu
{
    public class StorageUI
    {
        private Game _game;

        private AssetManager _assetManager;

        private SpriteBatch _spriteBatch;

        private BuildMenuUI _buildMenu;

        private IngameUI _ingameUI;

        private static TaskButton[] _taskMenuButtons;

        private static bool _showMenu;

        public delegate void ElementClicked(string element, MouseState mouseState);
        public event ElementClicked TaskMenuClicked;

        protected TextureAtlas _menuItems;

        #region Menu item names for texture atlas

        protected string _menu = "menuButton";
        protected string _subMenu = "subMenuButton";
        protected string _task = "taskButton";
        protected string _explanation = "explanationBox";

        #endregion

        public StorageUI(Game game, AssetManager assetManager, SpriteBatch spriteBatch, BuildMenuUI buildMenu, IngameUI ingameUI)
        {
            _game = game;
            _assetManager = assetManager;
            _spriteBatch = spriteBatch;
            _showMenu = false;
            _buildMenu = buildMenu;
            _buildMenu.SubMenuClicked += OnSubMenuClicked;
            _ingameUI = ingameUI;
            _ingameUI.MenuClicked += OnMenuClicked;
        }

        public void Initialize()
        {
            _menuItems = new TextureAtlas("taskButton", _assetManager.MenuItems, _assetManager.MenuItemsTextureAtlasXML);

            TaskButton deconstructButton = new TaskButton(_menuItems[_task], _assetManager.InGameFont, GameText.BuildMenu.DECONSTRUCT, _spriteBatch);
            TaskButton areaButton = new TaskButton(_menuItems[_task], _assetManager.InGameFont, GameText.BuildMenu.STORAGEAREA, _spriteBatch);
            TaskButton crateButton = new TaskButton(_menuItems[_task], _assetManager.InGameFont, GameText.BuildMenu.STORAGECRATE, _spriteBatch);
            TaskButton barrelButton = new TaskButton(_menuItems[_task], _assetManager.InGameFont, GameText.BuildMenu.STORAGEBARREL, _spriteBatch);

            _taskMenuButtons = new TaskButton[4];
            
            _taskMenuButtons[0] = deconstructButton;
            _taskMenuButtons[1] = areaButton;
            _taskMenuButtons[2] = crateButton;
            _taskMenuButtons[3] = barrelButton;

            int offset = (_game.GraphicsDevice.Viewport.Width / IngameUI.MenuItemsCount()) + 15;

            int heightIndex = _game.GraphicsDevice.Viewport.Height - 110; // Hardcoded for now

            int widthIndex = offset;

            for (int i = 0; i < _taskMenuButtons.GetLength(0); i++)
            {
                if ((widthIndex + _taskMenuButtons[i].Position.Width) >= _game.GraphicsDevice.Viewport.Width)
                {
                    heightIndex -= 70;
                    widthIndex = offset;
                }

                _taskMenuButtons[i].Position = new Rectangle(widthIndex, heightIndex, _menuItems[_task].Width, _menuItems[_task].Height);

                _taskMenuButtons[i].ClickEvent += OnTaskMenuClicked;

                widthIndex += _menuItems[_task].Width + 46;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_showMenu)
            {
                for (int i = 0; i < _taskMenuButtons.GetLength(0); i++)
                    _taskMenuButtons[i].Update();
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_showMenu)
            {
                _spriteBatch.Begin();

                for (int i = 0; i < _taskMenuButtons.GetLength(0); i++)
                    _taskMenuButtons[i].Draw();

                _spriteBatch.End();
            }
        }

        private void OnTaskMenuClicked(string element, MouseState mouseState)
        {
            TaskMenuClicked(element, mouseState);
        }

        private void OnSubMenuClicked(string element, MouseState mouseState)
        {
            if (element == GameText.BuildMenu.STORAGE)
            {
                if (_showMenu)
                    _showMenu = false;
                else
                    _showMenu = true;
            }
            else if (_showMenu)
                _showMenu = false;
        }

        private void OnMenuClicked(string element, MouseState mouseState)
        {
            if (_showMenu)
                _showMenu = false;
        }

        public static bool IsMouseOverMenu()
        {
            if (_showMenu)
            {
                MouseState currentMouseState = Mouse.GetState();

                for (int i = 0; i < _taskMenuButtons.GetLength(0); i++)
                {
                    Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                    // If mouse is intersecting with the button
                    if (_taskMenuButtons[i].Position.Contains(mousePosition))
                        return true;
                }
            }

            return false;
        }
    }
}
