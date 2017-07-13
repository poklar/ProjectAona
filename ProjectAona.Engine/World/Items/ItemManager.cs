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
            for (int i = 0; i < 5; i++)
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
                foreach (var items in _unstackedItems)
                {
                    List<List<IStackable>> inv = new List<List<IStackable>>();

                    inv.Add(items);

                    Tile tile = StockpileManager.AvailabeLocation(stockpile, items);

                    if (tile != null)
                    {
                        _jobManager.CreateJob(null, tile, inv);
                        //_unstackedItems.Remove(items);
                    }


                }

                // TODO: LOL remove this pls
                _unstackedItems.Clear();
            }
        }


    }
}
