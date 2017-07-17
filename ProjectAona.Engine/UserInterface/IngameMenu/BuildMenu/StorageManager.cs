using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Common;
using ProjectAona.Engine.Core;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.UserInterface.IngameMenu.BuildMenu
{
    public class StorageManager
    {
        private SpriteBatch _spriteBatch;

        private Camera _camera;

        private AssetManager _assetManager;

        private StorageUI _storageUI;

        private string _selectedElement;

        private SelectionArea _selectionArea;

        private SelectDeconstructStockpile _selectDeconstructStockpile;

        private SelectionType _selectionType;

        public StorageManager(SpriteBatch spriteBatch, Camera camera, AssetManager assetManager, StorageUI storageUI)
        {
            _spriteBatch = spriteBatch;
            _camera = camera;
            _assetManager = assetManager;
            _storageUI = storageUI;
            _storageUI.TaskMenuClicked += OnTaskClicked;
            _selectedElement = "";
            _selectionType = SelectionType.None;

            // TODO: Stockpile needs to get its own class where it'll be checked if the tile is a stockpile
            _selectionArea = new SelectionArea(_assetManager, _camera, _spriteBatch);
            _selectionArea.SelectionSelected += OnAreaSelected;
            _selectionArea.CancelledSelection += OnSelectionCancelled;

            _selectDeconstructStockpile = new SelectDeconstructStockpile(_assetManager, _camera, _spriteBatch);
            _selectDeconstructStockpile.DeconstructionSelected += OnDeconstructSelected;
            _selectDeconstructStockpile.CancelledSelection += OnSelectionCancelled;
        }

        public void Update(GameTime gameTime)
        {
            if (GameState.State == GameStateType.SELECTING)
            {
                switch (_selectionType)
                {
                    case SelectionType.Storage: _selectionArea.Update(); break;
                    case SelectionType.Deconstruct: _selectDeconstructStockpile.Update(); break;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (GameState.State == GameStateType.SELECTING)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);

                switch (_selectionType)
                {
                    case SelectionType.Storage: _selectionArea.Draw(); break;
                    case SelectionType.Deconstruct: _selectDeconstructStockpile.Draw(); break;
                }

                _spriteBatch.End();
            }            
        }

        private void OnTaskClicked(string element, MouseState mouseState)
        {
            if (element == GameText.BuildMenu.STORAGEAREA || element == GameText.BuildMenu.DECONSTRUCT)
            {
                GameState.State = GameStateType.SELECTING;

                _selectedElement = element;

                if (element == GameText.BuildMenu.STORAGEAREA)
                {
                    _selectionType = SelectionType.Storage;
                    _selectionArea.SelectArea(mouseState);
                }
                else
                {
                    _selectionType = SelectionType.Deconstruct;
                    _selectDeconstructStockpile.SelectArea(mouseState);
                }
            }
            else if (element == GameText.BuildMenu.STORAGECRATE)
            {

            }
            else if (element == GameText.BuildMenu.STORAGEBARREL)
            {

            }
        }

        private void OnAreaSelected(Dictionary<Rectangle, TextureRegion2D> selectedTiles)
        {
            if (_selectedElement == GameText.BuildMenu.STORAGEAREA)
            {
                List<Tile> tiles = new List<Tile>();

                foreach (Rectangle rectangle in selectedTiles.Keys)
                {
                    Tile tile = ChunkManager.TileAtWorldPosition(rectangle.X, rectangle.Y);

                    if (tile != null)
                        tiles.Add(tile);
                }

                StockpileManager.CreateStockpile(tiles);
            }

            _selectedElement = "";
            _selectionType = SelectionType.None;
            GameState.State = GameStateType.PLAYING;
        }

        private void OnDeconstructSelected(Tile tile)
        {
            StockpileManager.RemoveStockpile(tile);

            _selectedElement = "";
            _selectionType = SelectionType.None;
            GameState.State = GameStateType.PLAYING;
        }

        private void OnSelectionCancelled()
        {
            _selectedElement = "";
            _selectionType = SelectionType.None;
            GameState.State = GameStateType.PLAYING;
        }

        public void CreateStockpile()
        {

        }
    }
}
