using Microsoft.Xna.Framework;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Selection;
using System;
using System.Diagnostics;

namespace ProjectAona.Engine.World.NPC
{
    public class Minion : INPC, ISelectableInterface
    {
        private Vector2 _position;
        private Tile _currentTile;
        private bool _moving;

        // TODO: Should be a number
        public string ID { get; set; }

        public Vector2 Position { get { return _position; } set { _position = value; } }

        public Tile CurrentTile
        {
            get { return _currentTile; }
            protected set
            {
                if (_currentTile != null && _currentTile.Minions.Contains(this))
                    _currentTile.Minions.Remove(this);
                _currentTile = value; _currentTile.Minions.Add(this); 
            }
        }

        public Tile DestinationTile { get; protected set; }

        public float Speed { get; private set; }

        public delegate void MinionHandler(Minion minion);
        public event MinionHandler MinionChanged;

        public Minion(Tile tile, string id, float speed = 50)
        {
            CurrentTile = _currentTile = DestinationTile = tile;
            _position = tile.Position;
            ID = id;
            Speed = speed;;
            _moving = false;
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentTile == DestinationTile && !_moving)
                return;
            
            float distance = Vector2.Distance(CurrentTile.Position, DestinationTile.Position);

            Vector2 direction = Vector2.Normalize(DestinationTile.Position - CurrentTile.Position);

            _position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Vector2.Distance(CurrentTile.Position, _position) >= distance)
            {
                CurrentTile = DestinationTile;
                _position = CurrentTile.Position;
            }

            //if (MinionChanged != null)
            //    MinionChanged(this);
        }

        public void SetCurrentTile(Tile tile)
        {
            CurrentTile = tile;
        }

        public void SetDestinationTile(Tile tile)
        {
            DestinationTile = tile;
        }

        public string GetName()
        {
            return "Minion id: " + ID;
        }
    }
}
