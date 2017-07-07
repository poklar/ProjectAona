using Microsoft.Xna.Framework;
using ProjectAona.Engine.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.NPC
{
    public interface INPC
    {
        Vector2 Position { get; set; }

        Tile CurrentTile { get; }

        Tile DestinationTile { get; set; }
    }
}
