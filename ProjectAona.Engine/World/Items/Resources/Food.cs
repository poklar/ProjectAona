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

        public string GetName()
        {
            return "";
        }
    }
}
