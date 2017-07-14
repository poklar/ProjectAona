﻿using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Selection;
using System;
using System.Collections.Generic;

namespace ProjectAona.Engine.World
{
    public class Stockpile : ISelectableInterface
    {
        public Dictionary<Tile, List<IStackable>> Stack { get; set; }

        public Dictionary<Tile, int> ReservedItemCounter { get; set; }

        public Dictionary<Tile, IStackable> ReservedItem { get; set; }

        public Stockpile(List<Tile> tiles)
        {
            Stack = new Dictionary<Tile, List<IStackable>>();
            ReservedItemCounter = new Dictionary<Tile, int>();
            ReservedItem = new Dictionary<Tile, IStackable>();

            for (int i = 0; i < tiles.Count; i++)
            {
                Stack.Add(tiles[i], new List<IStackable>());
                ReservedItemCounter.Add(tiles[i], 0);
            }
        }

        public void AddItem(Tile tile, IStackable item)
        {
            // Check if there already is an item type in the array
        }

        public void RemoveItem(Tile tile, IStackable item)
        {

        }

        public int MaxStockpileItemValue(IStackable item)
        {


            return 0;
        }



        public string GetName()
        {
            return "Stockpile";
        }
    }
}
