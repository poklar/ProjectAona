using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.World.TerrainObjects
{
    public class Mineral
    {
        public Vector2 Position { get; set; }

        public MineralType Type { get; set; }

        public bool IsMined { get; set; }

        public Mineral(Vector2 position, MineralType type)
        {
            Position = position;
            Type = type;
            IsMined = false;
        }
    }
}
