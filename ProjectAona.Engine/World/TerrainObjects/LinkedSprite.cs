using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.World.TerrainObjects
{
    public abstract class LinkedSprite
    {
        public abstract bool HasNeighbor { get; set; }

        public abstract Vector2 Position { get; set; }

        public abstract LinkedSpriteType Type { get; set; }
    }
}
