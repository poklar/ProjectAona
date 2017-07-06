using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Common;
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
        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// The camera.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The chunk manager.
        /// </summary>
        private ChunkManager _chunkManager;

        /// <summary>
        /// The playing state interface.
        /// </summary>
        private PlayingStateInterface _playingStateInterface;

        /// <summary>
        /// The asset manager.
        /// </summary>
        private AssetManager _assetManager;

        /// <summary>
        /// The terrain manager.
        /// </summary>
        private TerrainManager _terrainManager;

        /// <summary>
        /// The selection area.
        /// </summary>
        private SelectWallArea _selectionArea;

        /// <summary>
        /// The selecting area.
        /// </summary>
        private bool _selectingArea;

        /// <summary>
        /// The selected build name.
        /// </summary>
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
            // Subcribe to the event
            _playingStateInterface.OnSubMenuClicked += OnBuildWall;
            _assetManager = assetManager;
            _terrainManager = terrainManager;
            _selectionArea = new SelectWallArea(_assetManager, _camera, _chunkManager, _spriteBatch, _terrainManager, this);
            // Subsscribe to the events
            _selectionArea.OnSelectionSelected += OnSelectionSelected;
            _selectionArea.OnSelectionCancelled += OnSelectionCancelled;
            _buildSelectionName = "";
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            // If player is selecting an area
            if (_selectingArea)
                // Update the class
                _selectionArea.Update();
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);

            // If the player is selecting an area
            if (_selectingArea)
                // Draw the components
                _selectionArea.Draw();

            _spriteBatch.End();
        }

        /// <summary>
        /// Called when player clicks [build wall].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="mouseState">State of the mouse.</param>
        private void OnBuildWall(string element, MouseState mouseState)
        {
            // If the player isn't selecting something already and wants to build a wood/brick/stone wall 
            if (!_selectingArea && element == GameText.BuildMenu.BUILDWOODWALL || element == GameText.BuildMenu.BUILDBRICKWALL || element == GameText.BuildMenu.BUILDSTONEWALL)
            {
                // Save the element name the player wants to build (ie stone wall)
                _buildSelectionName = element;
                // Select the area
                _selectionArea.SelectArea(mouseState);
                // Player is now selecting an area, set bool to true
                _selectingArea = true;
            }
            // If anything else happens 
            else
                // Cancel the selection
                _selectionArea.CancelSelection();
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

                    // If tile isn't null
                    if (tile != null)
                    {
                        // Name was wood wall
                        if (_buildSelectionName == GameText.BuildMenu.BUILDWOODWALL)
                            // Build a wood wall
                            TerrainManager.AddWall(LinkedSpriteType.WoodWall, tile);
                        // If name was brick wall
                        else if (_buildSelectionName == GameText.BuildMenu.BUILDBRICKWALL)
                            // Build a brick wall
                            TerrainManager.AddWall(LinkedSpriteType.BrickWall, tile);
                        // Else if the name was stone wall
                        else if (_buildSelectionName == GameText.BuildMenu.BUILDSTONEWALL)
                            // Build a stone wall
                            TerrainManager.AddWall(LinkedSpriteType.StoneWall, tile);
                    }
                }
            }

            // Player is done selecting an area to build now, set bool to false
            _selectingArea = false;
        }

        /// <summary>
        /// Called when [selection is cancelled].
        /// </summary>
        private void OnSelectionCancelled()
        {
            //Player is done selecting an area to build now, set bool to false
            _selectingArea = false;
        }

        /// <summary>
        /// Determines whether [is mouse over menu].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is mouse over menu]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMouseOverMenu()
        {
            // Return if true or not
            return _playingStateInterface.IsMouseOverMenu();
        }
    }
}
