using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.NPC;
using System.Collections.Generic;

namespace ProjectAona.Engine.World
{
    public class NPCManager
    {
        private Dictionary<string, Minion> _minions;

        private Texture2D _minionTexture;

        private int _IDcounter;

        private Camera _camera;

        private AssetManager _assetManager;

        private ChunkManager _chunkManager;

        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCManager"/> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="assetManager">The asset manager.</param>
        /// <param name="chunkManager">The chunk manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public NPCManager(Camera camera, AssetManager assetManager, ChunkManager chunkManager, SpriteBatch spriteBatch)
        {
            _camera = camera;
            _assetManager = assetManager;
            _chunkManager = chunkManager;
            _spriteBatch = spriteBatch;
            _IDcounter = 0;
            _minions = new Dictionary<string, Minion>();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            _minionTexture = _assetManager.NPCNormal;
            SpawnMinion();
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _camera.View);

            foreach (Minion minion in _minions.Values)
            {
                _spriteBatch.Draw(_minionTexture, minion.Position, Color.White);
            }

            _spriteBatch.End();
        }

        /// <summary>
        /// Spawns the minion.
        /// </summary>
        private void SpawnMinion()
        {
            // TODO: Make this random/at the middle of the map
            Tile tile = _chunkManager.TileAtWorldPosition(96, 64);
            tile = EmptyTile(tile);

            Minion minion = new Minion(tile, _IDcounter.ToString());
            tile.Minions.Add(minion);
            _minions.Add(_IDcounter.ToString(), minion);
            _IDcounter++;
        }

        /// <summary>
        /// Returns an empty tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns></returns>
        private Tile EmptyTile(Tile tile)
        {
            for (float x = tile.Position.X; x < Core.Engine.Instance.Configuration.World.MapWidth; x+=32)
            {
                tile.Position = new Vector2(x, tile.Position.Y);

                if (!tile.IsOccupied)
                    return tile;
            }

            // Move one tile down if everything was occupied
            tile.Position = new Vector2(tile.Position.X, tile.Position.Y + 32);

            // Recall this function until an empty tile is found
            return EmptyTile(tile);
        }
    }
}
