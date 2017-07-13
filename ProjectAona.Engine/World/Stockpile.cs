using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Selection;
using System;
using System.Collections.Generic;

namespace ProjectAona.Engine.World
{
    public class Stockpile : ISelectableInterface
    {
        public Dictionary<Tile, List<IStackable>> Stack { get; set; }

        public Stockpile(List<Tile> tiles)
        {
            Stack = new Dictionary<Tile, List<IStackable>>();

            for (int i = 0; i < tiles.Count; i++)
                Stack.Add(tiles[i], new List<IStackable>());
        }

        public void AddItem(Tile tile, IStackable item)
        {
            // Check if there already is an item type in the array
        }

        public void RemoveItem(Tile tile, IStackable item)
        {

        }

        public string GetName()
        {
            return "Stockpile";
        }
    }
}
