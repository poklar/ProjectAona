using ProjectAona.Engine.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.Items
{
    public interface IStackable
    {
        Tile Tile { get; set; }

        string ItemName { get; set; }

        int MovementCost { get; set; }

        int MaxStackSize { get; set; }
    }
}
