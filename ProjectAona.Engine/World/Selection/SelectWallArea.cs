using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Menu;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.World.Selection
{
    public class SelectWallArea : SelectionArea
    {
        public SelectWallArea(AssetManager assetManager, Camera camera, ChunkManager chunkManager, SpriteBatch spriteBatch, TerrainManager terrainManager, BuildMenuManager buildMenuManager)
            : base (assetManager, camera, chunkManager, spriteBatch, terrainManager, buildMenuManager)
        {

        }

        protected override void CalculateSelectedArea(MouseState mouseState)
        {
            _selectedTiles.Clear();

            Vector2 worldMousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), Matrix.Invert(_camera.View));

            Rectangle currentTilePosition = TilePosition(worldMousePosition);

            // TODO: Fix this
            if (_startTilePosition.X <= currentTilePosition.X && _startTilePosition.Y <= currentTilePosition.Y)
            {
                for (int x = _startTilePosition.X; x <= currentTilePosition.X; x += 32)
                    AddSelectedTile(x, _startTilePosition.Y);
            }
            else if (_startTilePosition.X <= currentTilePosition.X && _startTilePosition.Y >= currentTilePosition.Y)
            {
                for (int y = _startTilePosition.Y; y >= currentTilePosition.Y; y -= 32)
                    AddSelectedTile(_startTilePosition.X, y);
                
            }
            else if (_startTilePosition.X >= currentTilePosition.X && _startTilePosition.Y >= currentTilePosition.Y)
            {
                for (int x = _startTilePosition.X; x >= currentTilePosition.X; x -= 32)
                    AddSelectedTile(x, _startTilePosition.Y);

            }
            else if (_startTilePosition.X >= currentTilePosition.X && _startTilePosition.Y <= currentTilePosition.Y)
            {
                for (int y = _startTilePosition.Y; y <= currentTilePosition.Y; y += 32)
                    AddSelectedTile(_startTilePosition.X, y);                
            }
        }

    }
}
