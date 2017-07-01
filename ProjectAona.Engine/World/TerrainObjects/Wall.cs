using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.World.TerrainObjects
{
    public class Wall
    {
        public Vector2 Position { get; set; }

        public WallType WallType { get; set; }

        public bool IsDestructed { get; set; }

        public bool HasNeighbor { get; set; }

        public bool Visible { get; set; }

        public Wall(Vector2 position, WallType type)
        {
            Position = position;
            WallType = type;
            IsDestructed = false;
            HasNeighbor = false;
            Visible = false;
        }
    }
}
