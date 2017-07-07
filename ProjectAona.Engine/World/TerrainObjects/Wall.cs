using Microsoft.Xna.Framework;
using ProjectAona.Engine.World.Selection;

namespace ProjectAona.Engine.World.TerrainObjects
{
    public class Wall : LinkedSprite, ISelectableInterface
    {
        public override Vector2 Position { get; set; }

        public override LinkedSpriteType Type { get; set; }

        public override bool HasNeighbor { get; set; }

        public bool IsDestructed { get; set; }        

        public bool Visible { get; set; }

        public Wall(Vector2 position, LinkedSpriteType type)
        {
            Position = position;
            Type = type;
            IsDestructed = false;
            HasNeighbor = false;
            Visible = false;
        }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return Type.ToString();
        }
    }
}
