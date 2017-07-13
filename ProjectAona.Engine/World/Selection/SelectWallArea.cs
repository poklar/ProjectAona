using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ProjectAona.Engine.Input;

namespace ProjectAona.Engine.World.Selection
{
    public class SelectWallArea : SelectionArea
    {
        public SelectWallArea(AssetManager assetManager, Camera camera, SpriteBatch spriteBatch)
            : base (assetManager, camera, spriteBatch)
        {

        }

        protected override void CalculateSelectedArea()
        {
            _selectedTiles.Clear();

            Vector2 worldMousePosition = MouseManager.GetWorldMousePosition();

            Rectangle currentTilePosition = TilePosition(worldMousePosition);

            if (currentTilePosition.X > 160 )
            {

            }

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

        private bool MouseInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            var A = 1 / 2 * (-p1.Y * p2.X + p0.Y * (-p1.X + p2.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y);
            var sign = A < 0 ? -1 : 1;
            var s = (p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y) * sign;
            var t = (p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y) * sign;

            return s > 0 && t > 0 && (s + t) < 2 * A * sign;
        }
    }
}
