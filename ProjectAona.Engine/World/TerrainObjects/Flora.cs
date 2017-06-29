using Microsoft.Xna.Framework;
using System;
namespace ProjectAona.Engine.World.TerrainObjects
{
    public class Flora
    {
        public Vector2 Position { get; set; }

        public FloraType FloraType { get; set; }

        public bool IsCut { get; set; }

        // TODO: Add a regrowth timer? Especially for berries?

        public Flora(Vector2 position, FloraType type)
        {
            Position = position;
            FloraType = type;
            IsCut = false;
        }
    }
}
