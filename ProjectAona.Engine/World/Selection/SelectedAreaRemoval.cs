﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Input;

namespace ProjectAona.Engine.World.Selection
{
    class SelectedAreaRemoval : SelectionArea
    {
        public event SelectionClicked SelectionRemovalSelected;
        public override event SelectionCancelled CancelledSelection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedAreaRemoval"/> class.
        /// </summary>
        /// <param name="assetManager">The asset manager.</param>
        /// <param name="camera">The camera.</param>
        /// <param name="chunkManager">The chunk manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="terrainManager">The terrain manager.</param>
        /// <param name="buildMenuManager">The build menu manager.</param>
        public SelectedAreaRemoval(AssetManager assetManager, Camera camera, SpriteBatch spriteBatch)
            : base (assetManager, camera, spriteBatch)
        {

        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            MouseState currentMouseState = Mouse.GetState();

            if (!_tileSelected && currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 worldMousePosition = Vector2.Transform(new Vector2(currentMouseState.X, currentMouseState.Y), Matrix.Invert(_camera.View));
                _startTilePosition = TilePosition(worldMousePosition);
                _tileSelected = true;
            }

            // As long as the player holds the left mouse button
            if (_tileSelected && currentMouseState.LeftButton == ButtonState.Pressed)
                CalculateSelectedArea();

            // If the player releases the previous pressed mouse button
            if (_tileSelected && currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (_validSelection && !MouseManager.IsMouseOverMenu())
                    SelectionRemovalSelected(_selectedTiles);

                // Player is done selecting tiles
                _tileSelected = false;
            }

            // If player presses right mouse button during selection
            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                _tileSelected = false;
                CancelledSelection();
            }

            _previousMouseState = currentMouseState;
        }

        /// <summary>
        /// Adds the selected tile.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        protected override void AddSelectedTile(int x, int y)
        {
            // Check if tile is in world bounds
            if (ChunkManager.InWorldBounds(x, y))
            {
                bool isOccupied = TerrainManager.IsTileOccupiedByOre(x, y);

                bool isSelectionValid = true;
                _validSelection = true;

                if (isOccupied)
                {
                    _selectedTiles.Add(new Rectangle(x, y, 32, 32), _invalidSelectionTexture);
                    isSelectionValid = false;
                }
                else
                    _selectedTiles.Add(new Rectangle(x, y, 32, 32), _validSelectionTexture);

                if (!isSelectionValid)
                    _validSelection = isSelectionValid;
            }
        }

        public override void CancelSelection()
        {
            _tileSelected = false;
            CancelledSelection();
        }
    }
}
