using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Core;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.UserInterface.IngameMenu.BuildMenu;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Items.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

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
        public static void AddItem(Stockpile stockpile, List<IStackable> items)
        {
            bool stackFound = false;

            foreach (var stack in stockpile.Stack)
            {
                // Stack already exists
                if (stack.Value.Count != 0 &&
                    stack.Value.FirstOrDefault().ItemName == items.FirstOrDefault().ItemName &&
                    stack.Value.Count + items.Count <= items.FirstOrDefault().MaxStackSize)
                {
                    for (int i = 0; i < items.Count; i++)
                        stack.Value.Add(items[i]);

                    stack.Key.Item = items;

                    return;                   
                }

            }

            if (!stackFound)
            {
                foreach (var stack in stockpile.Stack)
                {
                    if (stack.Value.Count == 0)
                    {
                        for (int i = 0; i < items.Count; i++)
                            stack.Value.Add(items[i]);

                        stack.Key.Item = items;

                        return;
                    }
                }
            }

            // TODO: Stack is full, do something?
        }

        public static Tile AvailabeLocation(Stockpile stockpile, List<IStackable> items)
        {
            bool stackFound = false;

            foreach (var stack in stockpile.Stack)
            {
                // Stack already exists
                if (stack.Value.Count != 0 &&
                    stack.Value.FirstOrDefault().ItemName == items.FirstOrDefault().ItemName &&
                    stack.Value.Count + items.Count <= items.FirstOrDefault().MaxStackSize)
                {
                    return stack.Key;
                }

            }

            if (!stackFound)
            {
                foreach (var stack in stockpile.Stack)
                {
                    if (stack.Value.Count == 0)
                    {
                        for (int i = 0; i < items.Count; i++)
                            stack.Value.Add(items[i]);

                        return stack.Key;
                    }
                }
            }

            return null;
            // TODO: Stack is full, do something?
        }

        public static bool IsSpacevailable()
        {
            return false;
        }
    }
}
