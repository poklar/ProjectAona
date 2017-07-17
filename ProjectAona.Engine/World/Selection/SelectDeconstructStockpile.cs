using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Input;
using ProjectAona.Engine.Tiles;

namespace ProjectAona.Engine.World.Selection
{
    class SelectDeconstructStockpile : SelectionArea
    {
        public delegate void TileClicked(Tile tile);
        public event TileClicked DeconstructionSelected;
        public override event SelectionCancelled CancelledSelection;

        private Tile _deconstructor;

        public SelectDeconstructStockpile(AssetManager assetManager, Camera camera, SpriteBatch spriteBatch)
            : base (assetManager, camera, spriteBatch)
        {

        }

        public override void Update()
        {
            MouseState currentMouseState = Mouse.GetState();

            CalculateSelectedArea();

            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                if (_validSelection && _deconstructor != null && !MouseManager.IsMouseOverMenu())
                    DeconstructionSelected(_deconstructor);
            }

            // If player presses right mouse button during selection
            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                _tileSelected = false;
                CancelledSelection();
            }

            _previousMouseState = currentMouseState;
        }

        public override void Draw()
        {
            RenderSelectedTiles();
        }

        protected override void CalculateSelectedArea()
        {
            _selectedTiles.Clear();

            Vector2 worldMousePosition = MouseManager.GetWorldMousePosition();

            Rectangle currentTilePosition = TilePosition(worldMousePosition);

            AddSelectedTile(currentTilePosition.X, currentTilePosition.Y);
        }

        protected override void AddSelectedTile(int x, int y)
        {
            // Check if tile is in world bounds
            if (ChunkManager.InWorldBounds(x, y))
            {
                _deconstructor = ChunkManager.TileAtWorldPosition(x, y);

                bool isOccupied = false;

                if (_deconstructor.Stockpile != null)
                    isOccupied = true;

                bool isSelectionValid = false;
                _validSelection = true;

                // TODO: Hardcoding pixelcount
                if (isOccupied)
                {
                    _selectedTiles.Add(new Rectangle(x, y, 32, 32), _validSelectionTexture);
                    isSelectionValid = true;
                }
                else
                    _selectedTiles.Add(new Rectangle(x, y, 32, 32), _invalidSelectionTexture);

                if (!isSelectionValid)
                    _validSelection = isSelectionValid;
            }
        }
    }
}
