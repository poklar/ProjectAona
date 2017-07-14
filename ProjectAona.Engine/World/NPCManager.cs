using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Pathfinding;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.NPC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectAona.Engine.World
{
    public class NPCManager
    {
        private Dictionary<Minion, Texture2D> _minions;

        private Texture2D _minionTexture;

        private int _IDcounter;

        private Camera _camera;

        private AssetManager _assetManager;

        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCManager"/> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="assetManager">The asset manager.</param>
        /// <param name="chunkManager">The chunk manager.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public NPCManager(Camera camera, AssetManager assetManager, SpriteBatch spriteBatch)
        {
            _camera = camera;
            _assetManager = assetManager;
            _spriteBatch = spriteBatch;
            _IDcounter = 0;
            _minions = new Dictionary<Minion, Texture2D>();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            _minionTexture = _assetManager.NPCNormal;
            Texture2D text = _assetManager.NPCRanger;

            SpawnMinion(96, 64, _minionTexture);
            //SpawnMinion(128, 96, text);
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            if (_minions.Count != 0)
                foreach (Minion minion in _minions.Keys)
                    minion.Update(gameTime);
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _camera.View);

            if (_minions.Count != 0)
                foreach (var minion in _minions)
                    _spriteBatch.Draw(minion.Value, minion.Key.Position, Color.White);

            _spriteBatch.End();
        }

        /// <summary>
        /// Spawns the minion.
        /// </summary>
        private void SpawnMinion(int x, int y, Texture2D texture)
        {
            // TODO: Make this random/at the middle of the map
            Tile tile = ChunkManager.TileAtWorldPosition(x, y);
            tile = EmptyTile(tile);

            Minion minion = new Minion(tile, _IDcounter.ToString());
            minion.MinionChanged += OnMinionChanged;
            minion.InventoryNeeded += OnPickUpInventory;
            minion.RemoveInventory += OnRemoveInventory;
            _minions.Add(minion, texture);
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

        private void OnPickUpInventory(Minion minion)
        {
            if (minion.Job != null && minion.CurrentTile.Item.Count != 0)
            {
                foreach (var requiredItem in minion.Job.RequiredItems)
                {
                    // Get the number of required items
                    var result = requiredItem.Where(item => item.ItemName == minion.CurrentTile.Item.FirstOrDefault().ItemName);

                    if (result.Count() != 0)
                    {
                        List<IStackable> inventory = new List<IStackable>();

                        // Add the inventory to the minion and remove it from the tile (hence the reverse counter)
                        for (int i = result.Count() - 1; i >= 0; i--)
                        {
                            inventory.Add(minion.CurrentTile.Item[i]);
                            minion.CurrentTile.Item.RemoveAt(0);
                        }

                        minion.Inventory.Add(inventory);

                        return;
                    }
                }
            }

            // TODO: Throw error, no inventory here
        }

        private void OnRemoveInventory(Minion minion)
        {
            // TODO: Assumes stockpile has enough space FIX IT

            foreach (var inventory in minion.Inventory)
                StockpileManager.AddItem(minion, inventory);
        }

        public static void MoveTo(Minion minion, Tile destinationTile)
        {
                minion.SetDestinationTile(destinationTile);
        }

        private void OnMinionChanged(Minion minion)
        {

        }
    }
}
