using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Common;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items.Decor;
using ProjectAona.Engine.World.Items.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.Items
{
    public class ItemManager
    {
        private List<List<IStackable>> _unstackedItems;

        private JobManager _jobManager;

        public ItemManager(JobManager jobManager)
        {
            _jobManager = jobManager;
            _unstackedItems = new List<List<IStackable>>();
            StockpileManager.StockpileCreated += OnStockpileCreated;

            #region TEST ITEMS
            // TODO: REMOVE
            // Spawn a few items just for testing purposes

            List<Material> materials = new List<Material>();
            Tile tile = ChunkManager.TileAtWorldPosition(0, 0);
            for (int i = 0; i < 20; i++)
            {
                Material material = new Material(tile);
                material.LoadMaterial("Wood");

                tile.Item.Add(material);
            }

            _unstackedItems.Add(tile.Item);

            tile = ChunkManager.TileAtWorldPosition(32, 0);
            for (int i = 0; i < 10; i++)
            {
                Material material = new Material(tile);
                material.LoadMaterial("Dirt");

                tile.Item.Add(material);
            }

            _unstackedItems.Add(tile.Item);

            tile = ChunkManager.TileAtWorldPosition(64, 0);
            for (int i = 0; i < 25; i++)
            {
                Material material = new Material(tile);
                material.LoadMaterial("CoalOre");

                tile.Item.Add(material);
            }

            _unstackedItems.Add(tile.Item);

            materials.Clear();
            tile = ChunkManager.TileAtWorldPosition(128, 0);
            for (int i = 0; i < 100; i++)
            {
                Material material = new Material(tile);
                material.LoadMaterial("Wood");

                tile.Item.Add(material);
            }

            _unstackedItems.Add(tile.Item);
            #endregion
        }

        public static void CreateItem(Tile tile, IStackable item, string itemName)
        {
            if (item.GetType() == typeof (Material))
            {
                Material material = new Material(tile);
                material.LoadMaterial(itemName);
            }
            else if (item.GetType() == typeof(Food))
            {

            }
            else if (item.GetType() == typeof(Furniture))
            {

            }
        }

        private void OnStockpileCreated(Stockpile stockpile)
        {
            if (_unstackedItems.Count != 0)
            {
                List<List<IStackable>> toBeRemoved = new List<List<IStackable>>();

                foreach (var items in _unstackedItems)
                {
                    KeyValuePair<Tile, int> destination = StockpileManager.ReserveLocation(stockpile, items);

                    if (destination.Key != null)
                    {
                        List<List<IStackable>> requiredInventory = new List<List<IStackable>>();

                        // Create a copy for the job, so nothing will happen to when the item gets picked up by the minion
                        List<IStackable> itemsCopy = new List<IStackable>();

                        for (int i = 0; i < destination.Value; i++)
                            itemsCopy.Add(items[i]);

                        requiredInventory.Add(itemsCopy);

                        _jobManager.CreateJob(null, destination.Key, requiredInventory);

                        if (destination.Value == items.Count)
                            toBeRemoved.Add(items);
                    }
                }

                if (toBeRemoved.Count != 0)
                {
                    for (int i = 0; i < toBeRemoved.Count; i++)
                        _unstackedItems.Remove(toBeRemoved[i]);
                }
            }
        }
    }
}
