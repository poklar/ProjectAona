using Microsoft.Xna.Framework;
using ProjectAona.Engine.Pathfinding;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Selection;
using System.Diagnostics;

namespace ProjectAona.Engine.World.NPC
{
    public class Minion : INPC, ISelectableInterface
    {
        private Vector2 _position;
        private Tile _currentTile;
        private Tile _destinationTile;
        private Tile _nextTile;
        private AStar _aStar;

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

        public Tile DestinationTile
        {
            get
            {
                return _destinationTile;
            }
            set
            {
                if (_destinationTile != value)
                    _destinationTile = value;
                _aStar = null;
            }
        }

        public float Speed { get; private set; }

        public delegate void MinionHandler(Minion minion);
        public event MinionHandler MinionChanged;

        public Minion(Tile tile, string id, float speed = 50)
        {
            CurrentTile = _currentTile = DestinationTile = tile;
            _nextTile = null;
            _position = tile.Position;
            ID = id;
            Speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentTile == DestinationTile)
            {
                _aStar = null;
                return;
            }

            // currTile = The tile the minion is currently in (and may be in the process of leaving)
            // nextTile = The tile the minion is currently entering
            // destTile = Our final destination -- the minion never walks here directly, but instead use it for the pathfinding

            if (_nextTile == null || _nextTile == _currentTile)
            {
                if (_aStar == null || _aStar.Length() == 0)
                {
                    // Calculates a path from current to destination
                    _aStar = new AStar(_currentTile, DestinationTile);

                    // No path found
                    if (_aStar.Length() == 0)
                    {
                        //TODO: cancel job?
                        _nextTile = _destinationTile = _currentTile; // Do this in job?
                        Debug.WriteLine("cancel");
                        return;
                    }

                    // The first tile can be ignored, because that's where the minion currently is in
                    _nextTile = _aStar.Pop();
                }

                // Get the next location 
                _nextTile = _aStar.Pop();
            }

            float distance = Vector2.Distance(CurrentTile.Position, _nextTile.Position);

            Vector2 direction = Vector2.Normalize(_nextTile.Position - CurrentTile.Position);

            _position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Vector2.Distance(CurrentTile.Position, _position) >= distance)
            {
                CurrentTile = _nextTile;
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
            _destinationTile = tile;
        }

        public string GetName()
        {
            return "Minion id: " + ID;
        }
    }
}
