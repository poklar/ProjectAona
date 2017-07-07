using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Common;
using ProjectAona.Engine.Core;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.UserInterface;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.Selection;
using ProjectAona.Engine.World.TerrainObjects;
using System.Collections.Generic;

namespace ProjectAona.Engine.Menu
{
    public class BuildMenuManager
    {
        private SpriteBatch _spriteBatch;

        private Camera _camera;

        private ChunkManager _chunkManager;

        private PlayingStateInterface _playingStateInterface;

        private AssetManager _assetManager;

        private TerrainManager _terrainManager;

        private SelectWallArea _selectWallArea;

        private SelectedAreaRemoval _selectAreaRemoval;

        private SelectingAreaType _selectingType;

        private string _buildSelectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildMenuManager"/> class.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="camera">The camera.</param>
        /// <param name="chunkManager">The chunk manager.</param>
        /// <param name="playingStateInterface">The playing state interface.</param>
        /// <param name="assetManager">The asset manager.</param>
        /// <param name="terrainManager">The terrain manager.</param>
        public BuildMenuManager(SpriteBatch spriteBatch, Camera camera, ChunkManager chunkManager, PlayingStateInterface playingStateInterface, AssetManager assetManager, TerrainManager terrainManager)
        {
            // Setters
            _spriteBatch = spriteBatch;
            _camera = camera;
            _chunkManager = chunkManager;
            _playingStateInterface = playingStateInterface;
            _playingStateInterface.OnMenuClicked += OnRemoval;
            _playingStateInterface.OnSubMenuClicked += OnBuildWall;
            _assetManager = assetManager;
            _terrainManager = terrainManager;

            _selectWallArea = new SelectWallArea(_assetManager, _camera, _chunkManager, _spriteBatch, _terrainManager);
            _selectWallArea.OnSelectionSelected += OnSelectionSelected;
            _selectWallArea.OnSelectionCancelled += OnSelectionCancelled;

            _selectAreaRemoval = new SelectedAreaRemoval(_assetManager, _camera, _chunkManager, _spriteBatch, _terrainManager);
            _selectAreaRemoval.OnSelectionRemovalSelected += OnRemoveSelected;
            _selectAreaRemoval.OnSelectionCancelled += OnSelectionCancelled;

            _selectingType = SelectingAreaType.None;

            _buildSelectionName = "";
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            switch (_selectingType)
            {
                case SelectingAreaType.Wall: _selectWallArea.Update(); break;
                case SelectingAreaType.Removal: _selectAreaRemoval.Update(); break;
            }
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);

            switch (_selectingType)
            {
                case SelectingAreaType.Wall: _selectWallArea.Draw(); break;
                case SelectingAreaType.Removal: _selectAreaRemoval.Draw(); break;
            }

            _spriteBatch.End();
        }

        /// <summary>
        /// Called when player clicks [build wall].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        private void OnBuildWall(string element, MouseState mouseState)
        {
            GameState.State = GameStateType.SELECTING;

            // If the player isn't selecting something already and wants to build a wood/brick/stone wall 
            if (_selectingType != SelectingAreaType.Wall && element == GameText.BuildMenu.BUILDWOODWALL || element == GameText.BuildMenu.BUILDBRICKWALL || element == GameText.BuildMenu.BUILDSTONEWALL)
            {
                // Save the element name the player wants to build (ie stone wall)
                _buildSelectionName = element;
                _selectWallArea.SelectArea(mouseState);
                // Player is now selecting an area, set bool to true
                _selectingType = SelectingAreaType.Wall;
            }
            else
                _selectWallArea.CancelSelection();
        }

        /// <summary>
        /// Called when player clicks [remove].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        private void OnRemoval(string element, MouseState mouseState)
        {
            GameState.State = GameStateType.SELECTING;

            if (_selectingType != SelectingAreaType.Removal)
            {
                _selectAreaRemoval.SelectArea(mouseState);
                _selectingType = SelectingAreaType.Removal;
            }
            else
                _selectAreaRemoval.CancelSelection();
        }

        /// <summary>
        /// Called when a [selection is selected].
        /// </summary>
        /// <param name="selectedTiles">The selected tiles.</param>
        private void OnSelectionSelected(Dictionary<Rectangle, Texture2D> selectedTiles)
        {
            // If the name of the element that was passed through OnBuildWall is build wood/brick/stone
            if (_buildSelectionName == GameText.BuildMenu.BUILDWOODWALL || _buildSelectionName == GameText.BuildMenu.BUILDBRICKWALL || _buildSelectionName == GameText.BuildMenu.BUILDSTONEWALL)
            {
                // For each rectangle in selected tiles rectangle
                foreach (var rectangle in selectedTiles.Keys)
                {
                    // Get the tile by selecting the start position of the rectangle
                    Tile tile = _chunkManager.TileAtWorldPosition(rectangle.X, rectangle.Y);
                    
                    if (tile != null)
                    {
                        if (_buildSelectionName == GameText.BuildMenu.BUILDWOODWALL)
                            TerrainManager.AddWall(LinkedSpriteType.WoodWall, tile);
                        else if (_buildSelectionName == GameText.BuildMenu.BUILDBRICKWALL)
                            TerrainManager.AddWall(LinkedSpriteType.BrickWall, tile);
                        else if (_buildSelectionName == GameText.BuildMenu.BUILDSTONEWALL)
                            TerrainManager.AddWall(LinkedSpriteType.StoneWall, tile);
                    }
                }
            }
            
            _selectingType = SelectingAreaType.None;
            GameState.State = GameStateType.PLAYING;
        }

        private void OnRemoveSelected(Dictionary<Rectangle, Texture2D> selectedTiles)
        {
            // For each rectangle in selected tiles rectangle
            foreach (var rectangle in selectedTiles.Keys)
            {
                // Get the tile by selecting the start position of the rectangle
                Tile tile = _chunkManager.TileAtWorldPosition(rectangle.X, rectangle.Y);

                if (tile != null && tile.IsOccupied)
                {
                    if (tile.Flora != null)
                        _terrainManager.RemoveFlora(tile);
                    else if (tile.Wall != null)
                        _terrainManager.RemoveWall(tile);
                }
            }

            _selectingType = SelectingAreaType.None;
            GameState.State = GameStateType.PLAYING;
        }

        /// <summary>
        /// Called when [selection is cancelled].
        /// </summary>
        private void OnSelectionCancelled()
        {
            _selectingType = SelectingAreaType.None;
            GameState.State = GameStateType.PLAYING;
        }
    }

    public enum SelectingAreaType
    {
        Wall,
        Removal,
        None
    }
}
