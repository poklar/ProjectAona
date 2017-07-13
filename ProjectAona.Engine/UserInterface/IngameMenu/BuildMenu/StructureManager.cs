using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Common;
using ProjectAona.Engine.Core;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Selection;
using ProjectAona.Engine.World.TerrainObjects;
using System;
using System.Collections.Generic;

namespace ProjectAona.Engine.UserInterface.IngameMenu.BuildMenu
{
    public class StructureManager
    {
        private SpriteBatch _spriteBatch;

        private Camera _camera;

        private StructureUI _structureUI;

        private AssetManager _assetManager;
       
        private JobManager _jobManager;

        private SelectWallArea _selectWallArea;

        private SelectedAreaRemoval _selectAreaRemoval;

        private SelectCancelJobArea _selectCancelJobArea;

        private SelectionType _selectionType;

        private string _selectedElement;

        public StructureManager(SpriteBatch spriteBatch, Camera camera, StructureUI structureUI, AssetManager assetManager, JobManager jobManager)
        {
            _spriteBatch = spriteBatch;
            _camera = camera;

            _structureUI = structureUI;
            _structureUI.TaskMenuClicked += OnTaskClicked;
            
            _assetManager = assetManager;
            _jobManager = jobManager;

            _selectWallArea = new SelectWallArea(_assetManager, _camera, _spriteBatch);
            _selectWallArea.SelectionSelected += OnSelectionSelected;
            _selectWallArea.CancelledSelection += OnSelectionCancelled;

            _selectAreaRemoval = new SelectedAreaRemoval(_assetManager, _camera, _spriteBatch);
            _selectAreaRemoval.SelectionRemovalSelected += OnDeconstructSelected;
            _selectAreaRemoval.CancelledSelection += OnSelectionCancelled;

            _selectCancelJobArea = new SelectCancelJobArea(assetManager, _camera, _spriteBatch);
            _selectCancelJobArea.CancelJobAreaSelected += OnCancelJobSelected;
            _selectCancelJobArea.CancelledSelection += OnSelectionCancelled;

            _selectionType = SelectionType.None;

            _selectedElement = "";
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Check if state is selecting after I implent doors

            switch (_selectionType)
            {
                case SelectionType.Wall: _selectWallArea.Update(); break;
                case SelectionType.Deconstruct: _selectAreaRemoval.Update(); break;
                case SelectionType.Cancel: _selectCancelJobArea.Update(); break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            // TODO: Check if state is selecting after I implent doors

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);

            switch (_selectionType)
            {
                case SelectionType.Wall: _selectWallArea.Draw(); break;
                case SelectionType.Deconstruct: _selectAreaRemoval.Draw(); break;
                case SelectionType.Cancel: _selectCancelJobArea.Draw(); break;
            }

            _spriteBatch.End();
        }

        private void OnTaskClicked(string element, MouseState mouseState)
        {
            GameState.State = GameStateType.SELECTING;

            // If the player isn't selecting something already and wants to build a wood/brick/stone wall 
            if (_selectionType != SelectionType.Wall && element == GameText.BuildMenu.WOODWALL || element == GameText.BuildMenu.BRICKWALL || element == GameText.BuildMenu.STONEWALL)
            {
                _selectionType = SelectionType.Wall;

                // Save the element name the player wants to build (ie stone wall)
                _selectedElement = element;
                _selectWallArea.SelectArea(mouseState);
            }
            else if (_selectionType != SelectionType.Cancel && element == GameText.BuildMenu.CANCEL) // TODO: Should I check for selection type??
            {
                _selectedElement = element;

                _selectionType = SelectionType.Cancel;

                _selectCancelJobArea.SelectArea(mouseState);
            }
            else if (_selectionType != SelectionType.Deconstruct && element == GameText.BuildMenu.DECONSTRUCT)
            {
                _selectedElement = element;

                _selectionType = SelectionType.Deconstruct;

                _selectAreaRemoval.SelectArea(mouseState);
            }
        }

        private void OnSelectionSelected(Dictionary<Rectangle, TextureRegion2D> selectedTiles)
        {
            // If the name of the element that was passed through OnBuildWall is build wood/brick/stone
            if (_selectedElement == GameText.BuildMenu.WOODWALL || _selectedElement == GameText.BuildMenu.BRICKWALL || _selectedElement == GameText.BuildMenu.STONEWALL)
            {
                // For each rectangle in selected tiles rectangle
                foreach (Rectangle rectangle in selectedTiles.Keys)
                {
                    // Get the tile by selecting the start position of the rectangle
                    Tile tile = ChunkManager.TileAtWorldPosition(rectangle.X, rectangle.Y);

                    if (tile != null)
                    {
                        Wall wall;
                        if (_selectedElement == GameText.BuildMenu.WOODWALL)
                        {
                            wall = new Wall(tile.Position, LinkedSpriteType.WoodWall);
                            _jobManager.CreateJob(wall, tile);
                        }
                        else if (_selectedElement == GameText.BuildMenu.BRICKWALL)
                        {
                            wall = new Wall(tile.Position, LinkedSpriteType.BrickWall);
                            _jobManager.CreateJob(wall, tile);
                        }
                        else if (_selectedElement == GameText.BuildMenu.STONEWALL)
                        {
                            wall = new Wall(tile.Position, LinkedSpriteType.StoneWall);
                            _jobManager.CreateJob(wall, tile);
                        }
                    }
                }
            }

            _selectedElement = "";
            _selectionType = SelectionType.None;
            GameState.State = GameStateType.PLAYING;
        }

        private void OnCancelJobSelected(Dictionary<Rectangle, TextureRegion2D> selectedTiles)
        {
            foreach (Rectangle rectangle in selectedTiles.Keys)
            {
                Tile tile = ChunkManager.TileAtWorldPosition(rectangle.X, rectangle.Y);

                if (tile != null && tile.Blueprint != null)
                    _jobManager.CancelJob(tile.Blueprint, tile);
            }

            _selectedElement = "";
            _selectionType = SelectionType.None;
            GameState.State = GameStateType.PLAYING;
        }

        private void OnDeconstructSelected(Dictionary<Rectangle, TextureRegion2D> selectedTiles)
        {
            // For each rectangle in selected tiles rectangle
            foreach (Rectangle rectangle in selectedTiles.Keys)
            {
                // Get the tile by selecting the start position of the rectangle
                Tile tile = ChunkManager.TileAtWorldPosition(rectangle.X, rectangle.Y);

                if (tile != null && tile.IsOccupied && tile.Wall != null)
                    _jobManager.CreateJob(tile.Wall, tile);
                // TODO: Add door (Is door a furniture?)
            }

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
    }
}
