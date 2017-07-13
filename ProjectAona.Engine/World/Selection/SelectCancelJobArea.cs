using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.Selection
{
    public class SelectCancelJobArea : SelectionArea
    {
        public event SelectionClicked CancelJobAreaSelected;
        public override event SelectionCancelled CancelledSelection;

        public SelectCancelJobArea(AssetManager assetManager, Camera camera, SpriteBatch spriteBatch)
            : base (assetManager, camera, spriteBatch)
        {

        }

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
                    CancelJobAreaSelected(_selectedTiles);

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

        protected override void AddSelectedTile(int x, int y)
        {
            // Check if tile is in world bounds
            if (ChunkManager.InWorldBounds(x, y))
            {
                _selectedTiles.Add(new Rectangle(x, y, 32, 32), _validSelectionTexture);

                _validSelection = true;
            }
        }
    }
}
