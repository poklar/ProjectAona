using System;
using Microsoft.Xna.Framework;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.World.Selection;

namespace ProjectAona.Engine.World.TerrainObjects
{
    public class Wall : LinkedSprite, ISelectableInterface, IQueueable
    {
        private const float _movementCost = 100000; // TODO: Should have a movementspeed of 0, and fix it in A*

        public override Vector2 Position { get; set; }

        public override LinkedSpriteType Type { get; set; }

        public override bool HasNeighbor { get; set; }

        public override float MovementCost { get { return _movementCost; } }

        public bool IsDestructed { get; set; }        

        public bool Visible { get; set; }

        public BlueprintType BlueprintType { get; set; }

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
