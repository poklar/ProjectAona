using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Items.Resources;
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
        public static event StockpileEvent StockpileDeleted;


        public StockpileManager()
        {
            _stockpiles = new List<Stockpile>();
        }

        public static void CreateStockpile(List<Tile> tiles)
        {
            var rng = new Random();

            // Randomnize the tiles 
            List<Tile> tilesCopy = tiles.OrderBy(a => rng.Next()).ToList();

            Stockpile stockpile = new Stockpile(tilesCopy);

            for (int i = 0; i < tilesCopy.Count; i++)
                tilesCopy[i].Stockpile = stockpile;            

            _stockpiles.Add(stockpile);

            StockpileCreated(stockpile);
        }

        public static void RemoveStockpile(Tile tile)
        {
            for (int j = 0; j < _stockpiles.Count; j++)
            {
                // When a stack of the stockpile contains the current tile
                if (_stockpiles[j].Stack.ContainsKey(tile))
                {
                    List<Tile> stockpileTiles = new List<Tile>(_stockpiles[j].Stack.Keys);

                    // Set the tile's stockpile to null
                    for (int k = 0; k < _stockpiles[j].Stack.Count; k++)
                        stockpileTiles[k].Stockpile = null;

                    // Call an event so all the loose objects will saved in the item manager
                    StockpileDeleted(_stockpiles[j]);

                    // Dete stockpile
                    _stockpiles[j] = null;
                    _stockpiles.RemoveAt(j);

                    return;
                }
            }
        }

        // TODO: Split items if the stack gets full?
        public static void AddItem(Minion minion)
        {
            Stockpile stockpile = minion.CurrentTile.Stockpile;
            IStackable inventory = minion.Inventory.Keys.FirstOrDefault(); // TODO: Bad practice, what if the minion has more than one inv?
            int itemCounter = minion.Inventory.Values.FirstOrDefault();
            int itemIndex = 0;

            foreach (var stack in stockpile.Stack)
            {
                // Stack already exists
                if (stack.Value.Count != 0 &&
                    stack.Value.FirstOrDefault().ItemName == minion.Inventory.Keys.FirstOrDefault().ItemName)
                {
                    int availableSpace = minion.Inventory.Keys.FirstOrDefault().MaxStackSize - stack.Value.Count;

                    stockpile.ReservedItemCounter[stack.Key] = 0;
                    stockpile.ReservedItem[stack.Key] = null;
                    
                    IStackable invCopy = inventory.Clone();

                    invCopy.Tile = stack.Key;

                    // Only a part of the items will be able to be placed
                    if (availableSpace < itemCounter)
                    {
                        itemCounter -= availableSpace;
                        
                        while (itemIndex < availableSpace)
                        {
                            Inventorize(stack, minion, inventory, invCopy);

                            itemIndex++;
                        }

                        itemIndex = 0;             
                        
                    }
                    // Everything fits into the the tile
                    else
                    {
                        while (itemIndex < itemCounter)
                        {
                            Inventorize(stack, minion, inventory, invCopy);

                            itemIndex++;
                        }

                        foreach (var item in minion.Inventory.ToArray())
                        {
                            if (item.Value == 0)
                                minion.Inventory.Remove(item.Key);
                        }                        

                        return;
                    }
                }
            }

            // Run through the stockpile again to find an empty stack
            foreach (var stack in stockpile.Stack)
            {
                if (stack.Value.Count == 0)
                {               
                    int availableSpace = minion.Inventory.Keys.FirstOrDefault().MaxStackSize - stack.Value.Count;

                    IStackable invCopy = inventory.Clone();

                    invCopy.Tile = stack.Key;

                    stockpile.ReservedItemCounter[stack.Key] = 0;
                    stockpile.ReservedItem[stack.Key] = null;

                    if (availableSpace < itemCounter)
                    {
                        itemCounter -= availableSpace;
                       
                        while (itemIndex < availableSpace)
                        {
                            Inventorize(stack, minion, inventory, invCopy);

                            itemIndex++;
                        }

                        itemIndex = 0;
                    }
                    else
                    {
                        while (itemIndex < itemCounter)
                        {
                            Inventorize(stack, minion, inventory, invCopy);

                            itemIndex++;
                        }

                        if (itemIndex == itemCounter)
                            break;
                    }
                }
            }

            foreach (var item in minion.Inventory.ToArray())
            {
                if (item.Value == 0)
                    minion.Inventory.Remove(item.Key);
            }
        }

        private static void Inventorize(KeyValuePair<Tile, List<IStackable>> stack, Minion minion, IStackable inventory, IStackable invCopy)
        {
            stack.Value.Add(invCopy);
            stack.Key.Item.Add(invCopy);
            minion.Inventory[inventory] -= 1;
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
                            stockpile.ReservedItemCounter[stack.Key] += availableSpace;
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
                        int availableSpace = items.FirstOrDefault().MaxStackSize - stack.Value.Count;

                        stockpile.ReservedItem[stack.Key] = items.FirstOrDefault();

                        if (availableSpace < itemCounter)
                        {
                            itemCounter -= availableSpace;
                            stockpile.ReservedItemCounter[stack.Key] = availableSpace;
                        }
                        else
                        {
                            stockpile.ReservedItemCounter[stack.Key] = itemCounter;

                            return new KeyValuePair<Tile, int>(stack.Key, items.Count);
                        }
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
