using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.World.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.Items.Decor
{
    public class Furniture : ISelectableInterface, IQueueable 
    {
        public string ItemName { get; set; }

        private const int _maxStackSize = 1;

        public int MovementCost { get; set; }

        public int MaxStackSize { get; set; }

        public string GetName()
        {
            return "";
        }
    }
}
