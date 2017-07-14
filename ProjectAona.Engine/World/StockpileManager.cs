using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.NPC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.World
{
    public class StockpileManager
    {
        private static List<Stockpile> _stockpiles;

        public delegate void StockpileEvent(Stockpile stockpile);
        public static event StockpileEvent StockpileCreated;


        public StockpileManager()
        {
            _stockpiles = new List<Stockpile>();
        }

        public static void CreateStockpile(List<Tile> tiles)
        {
            var rng = new Random();

            // Randomnize the tiles 
            tiles.OrderBy(a => rng.Next());

            Stockpile stockpile = new Stockpile(tiles);

            for (int i = 0; i < tiles.Count; i++)
                tiles[i].Stockpile = stockpile;            

            _stockpiles.Add(stockpile);

            StockpileCreated(stockpile);
        }

        public static void RemoveStockpile(List<Tile> tiles)
        {

        }

        // TODO: Split items if the stack gets full?
        public static void AddItem(Minion minion, List<IStackable> items)
        {
            Stockpile stockpile = minion.CurrentTile.Stockpile;
            int itemCounter = items.Count;
            int itemIndex = 0;

            foreach (var stack in stockpile.Stack)
            {
                // Stack already exists
                if (stack.Value.Count != 0 &&
                    stack.Value.FirstOrDefault().ItemName == items.FirstOrDefault().ItemName)
                {
                    int availableSpace = items.FirstOrDefault().MaxStackSize - stack.Value.Count;

                    // Only a part of the items will be able to be placed
                    if (availableSpace < itemCounter)
                    {
                        itemCounter -= availableSpace;

                        stockpile.ReservedItemCounter[stack.Key] = 0;
                        stockpile.ReservedItem[stack.Key] = null;

                        while (itemIndex < itemCounter)
                        {
                            stack.Value.Add(items[itemIndex]);
                            stack.Key.Item.Add(items[itemIndex]);
                            items[itemIndex].Tile = stack.Key;

                            itemIndex++;
                        }
                    }
                    // Everything fits into the the tile
                    else
                    {
                        stockpile.ReservedItemCounter[stack.Key] = 0;
                        stockpile.ReservedItem[stack.Key] = null;

                        while (itemIndex < itemCounter)
                        {
                            stack.Value.Add(items[itemIndex]);
                            stack.Key.Item.Add(items[itemIndex]);
                            items[itemIndex].Tile = stack.Key;

                            itemIndex++;
                        }

                        // Look the current list up from the minion and remove the item
                        for (int i = 0; i < itemIndex; i++)
                            minion.Inventory.Find(list => list == items).RemoveAt(0); // TODO: Is this proper?

                        return;
                    }
                }

            }

            // Run through the stockpile again to find an empty stack
            foreach (var stack in stockpile.Stack)
            {
                if (stack.Value.Count == 0)
                {
                    // TODO: Check if items > maxstacksize
                    while (itemIndex < itemCounter)
                    {
                        stack.Value.Add(items[itemIndex]);
                        stack.Key.Item.Add(items[itemIndex]);
                        items[itemIndex].Tile = stack.Key;

                        itemIndex++;
                    }
                }
            }

            // Look the current list up from the minion and remove the item
            
            for (int i = 0; i < itemIndex; i++)
                minion.Inventory.Find(list => list == items).RemoveAt(0);
        }

        public static KeyValuePair<Tile, int> ReserveLocation(Stockpile stockpile, List<IStackable> items)
        {
            if (items.Count != 0)
            {
                int itemCounter = items.Count;
                Tile location = null;

                foreach (var stack in stockpile.Stack)
                {
                    // Stack already exists
                    if (stack.Value.Count + stockpile.ReservedItemCounter[stack.Key] != 0 &&
                        stockpile.ReservedItem[stack.Key].ItemName == items.FirstOrDefault().ItemName &&
                        stack.Value.Count + stockpile.ReservedItemCounter[stack.Key] < items.FirstOrDefault().MaxStackSize)
                    {
                        int availableSpace = items.FirstOrDefault().MaxStackSize - stack.Value.Count - stockpile.ReservedItemCounter[stack.Key];

                        if (availableSpace < itemCounter)
                        {
                            itemCounter -= availableSpace;
                            stockpile.ReservedItemCounter[stack.Key] = itemCounter;
                            location = stack.Key;
                        }
                        else
                        {
                            stockpile.ReservedItemCounter[stack.Key] = itemCounter;

                            return new KeyValuePair<Tile, int>(stack.Key, items.Count);
                        }
                    }         
                }

                // Not all items were able to be placed, check if there are empty tiles available
                foreach (var stack in stockpile.Stack)
                {
                    if (stack.Value.Count + stack.Value.Count + stockpile.ReservedItemCounter[stack.Key] == 0)
                    {
                        stockpile.ReservedItemCounter[stack.Key] = itemCounter;
                        stockpile.ReservedItem[stack.Key] = items.FirstOrDefault();

                        return new KeyValuePair<Tile, int>(stack.Key, items.Count);
                    }
                }
                
                // Not all items were able to be placed and no empty tiles left
                if (items.Count > itemCounter)
                    return new KeyValuePair<Tile, int>(location, items.Count - itemCounter);
            }

            return new KeyValuePair<Tile, int>(null, 0);
            // TODO: Stack is full, do something?
        }

        public static bool IsSpacevailable()
        {
            return false;
        }
    }
}
