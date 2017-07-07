using Microsoft.Xna.Framework;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Selection;

namespace ProjectAona.Engine.World.NPC
{
    public class Minion : INPC, ISelectableInterface
    {
        // TODO: Should be a number
        public string ID { get; set; }

        public Vector2 Position { get; set; }

        public Tile CurrentTile { get; protected set; }

        public Tile DestinationTile { get; set; }

        public Minion(Tile tile, string id)
        {
            CurrentTile = DestinationTile = tile;
            Position = tile.Position;
            ID = id;
        }

        public string GetName()
        {
            return "Minion id: " + ID;
        }
    }
}
