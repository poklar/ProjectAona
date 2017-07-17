using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.Items.Resources
{
    public class Food : IStackable, ISelectableInterface, IQueueable 
    {
        public Tile Tile { get; set; }

        public string ItemName { get; set; }
      
        public int MovementCost { get; set; }

        public int MaxStackSize { get; set; }

        public Food(Tile tile)
        {
            Tile = tile;
        }
        
        // Copy constructor
        protected Food(IStackable other)
        {
            Tile = other.Tile;
            ItemName = other.ItemName;
            MovementCost = other.MovementCost;
            MaxStackSize = other.MaxStackSize;
        }

        public IStackable Clone()
        {
            return new Food(this);
        }

        public string GetName()
        {
            return "";
        }
    }
}
