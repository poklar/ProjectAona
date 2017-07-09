using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Input;
using ProjectAona.Engine.Tiles;
using System.Collections.Generic;

namespace ProjectAona.Engine.World
{
    public class SelectionArea
    {
        protected ChunkManager _chunkManager;

        protected TerrainManager _terrainManager;

        protected Camera _camera;

        private SpriteBatch _spriteBatch;

        protected Dictionary<Rectangle, Texture2D> _selectedTiles;

        protected bool _tileSelected;

        protected bool _validSelection;

        protected MouseState _previousMouseState;

        protected Rectangle _startTilePosition;

        protected Texture2D _validSelectionTexture;
        protected Texture2D _invalidSelectionTexture;

        public delegate void SelectionClicked(Dictionary<Rectangle, Texture2D> selectedTiles);
        public event SelectionClicked OnSelectionSelected;

        public delegate void SelectionCancelled();
        public virtual event SelectionCancelled OnSelectionCancelled;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionArea"/> class.
        /// </summary>
        /// <param name="assetManager">The asset manager.</param>
        /// <param name="camera">The camera.</param>
        /// <param name="chunkManager">The chunk manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="terrainManager">The terrain manager.</param>
        /// <param name="buildMenuManager">The build menu manager.</param>
        public SelectionArea(AssetManager assetManager, Camera camera, ChunkManager chunkManager, SpriteBatch spriteBatch, TerrainManager terrainManager)
        {
            // Setters
            _selectedTiles = new Dictionary<Rectangle, Texture2D>();
            _chunkManager = chunkManager;
            _terrainManager = terrainManager;
            _tileSelected = false;
            _previousMouseState = Mouse.GetState();
            _validSelectionTexture = assetManager.Selection;
            _invalidSelectionTexture = assetManager.InvalidSelection;
            _spriteBatch = spriteBatch;
            _camera = camera;
            _validSelection = true;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public virtual void Update()
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
                    OnSelectionSelected(_selectedTiles);

                // Player is done selecting tiles
                _tileSelected = false;
            }

            // If player presses right mouse button during selection
            if (_tileSelected && currentMouseState.RightButton == ButtonState.Pressed)
            {
                _tileSelected = false;
                OnSelectionCancelled();
            }

            _previousMouseState = currentMouseState;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            // If there isn't any tile selected yet
            if (!_tileSelected)
                RenderSingleSelection();
            else
                RenderSelectedTiles();
        }

        /// <summary>
        /// Selects the area.
        /// </summary>
        /// <param name="mouseState">State of the mouse.</param>
        public void SelectArea(MouseState mouseState)
        {
            _previousMouseState = mouseState;
        }

        /// <summary>
        /// Cancels the selection.
        /// </summary>
        public virtual void CancelSelection()
        {
            _tileSelected = false;
            OnSelectionCancelled();
        }

        /// <summary>
        /// Calculates the selected area.
        /// </summary>
        protected virtual void CalculateSelectedArea()
        {
            _selectedTiles.Clear();

            Vector2 worldMousePosition = MouseManager.GetWorldMousePosition();

            Rectangle currentTilePosition = TilePosition(worldMousePosition);
            
            // Fills up a rectangle with tiles depending the position of the current tile (currentTilePosition)
            // In steps of 32, the pixel count 
            // TODO: Don't hardcode pixelcount
            if (_startTilePosition.X <= currentTilePosition.X && _startTilePosition.Y <= currentTilePosition.Y)
            {
                for (int x = _startTilePosition.X; x <= currentTilePosition.X; x+=32)
                {
                    for (int y = _startTilePosition.Y; y <= currentTilePosition.Y; y+=32)
                        AddSelectedTile(x, y);
                }
            }
            else if (_startTilePosition.X <= currentTilePosition.X && _startTilePosition.Y >= currentTilePosition.Y)
            {
                for (int x = _startTilePosition.X; x <= currentTilePosition.X; x+=32)
                {
                    for (int y = _startTilePosition.Y; y >= currentTilePosition.Y; y-=32)
                        AddSelectedTile(x, y);
                }
            }
            else if (_startTilePosition.X >= currentTilePosition.X && _startTilePosition.Y >= currentTilePosition.Y)
            {
                for (int x = _startTilePosition.X; x >= currentTilePosition.X; x-=32)
                {
                    for (int y = _startTilePosition.Y; y >= currentTilePosition.Y; y-=32)
                        AddSelectedTile(x, y);
                }
            }
            else if (_startTilePosition.X >= currentTilePosition.X && _startTilePosition.Y <= currentTilePosition.Y)
            {
                for (int x = _startTilePosition.X; x >= currentTilePosition.X; x-=32)
                {
                    for (int y = _startTilePosition.Y; y <= currentTilePosition.Y; y+=32)
                        AddSelectedTile(x, y);
                }
            }
        }

        /// <summary>
        /// Adds the selected tile.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        protected virtual void AddSelectedTile(int x, int y)
        {
            // Check if tile is in world bounds
            if (_chunkManager.InWorldBounds(x, y))
            {
                bool isOccupied = _terrainManager.IsTileOccupiedByWall(x, y);

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

        /// <summary>
        /// Renders the selected tiles.
        /// </summary>
        protected void RenderSelectedTiles()
        {
            foreach ( var tile in _selectedTiles)
                _spriteBatch.Draw(tile.Value, new Vector2(tile.Key.X, tile.Key.Y), Color.White);
        }

        /// <summary>
        /// Renders the single selection.
        /// </summary>
        protected void RenderSingleSelection()
        {
            MouseState currentMouseState = Mouse.GetState();

            Vector2 worldMousePosition = Vector2.Transform(new Vector2(currentMouseState.X, currentMouseState.Y), Matrix.Invert(_camera.View));

            Rectangle tilePosition = TilePosition(worldMousePosition);

            if (!tilePosition.IsEmpty && tilePosition.Contains(worldMousePosition.X, worldMousePosition.Y))
                _spriteBatch.Draw(_validSelectionTexture, new Vector2(tilePosition.X, tilePosition.Y), Color.White);
        }

        /// <summary>
        /// Rectangle position of the tile.
        /// </summary>
        /// <param name="worldMousePosition">The world mouse position.</param>
        /// <returns></returns>
        protected Rectangle TilePosition(Vector2 worldMousePosition)
        {
            Tile tile = _chunkManager.TileAtWorldPosition((int)worldMousePosition.X, (int)worldMousePosition.Y);

            Rectangle tilePosition = new Rectangle();

            if (tile != null)
                tilePosition = new Rectangle((int)tile.Position.X, (int)tile.Position.Y, 32, 32);

            return tilePosition;
        }
    }
}
