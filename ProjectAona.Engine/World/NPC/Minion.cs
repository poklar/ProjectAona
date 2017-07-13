using Microsoft.Xna.Framework;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Pathfinding;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Selection;
using System.Collections.Generic;
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
        private Job _job;
        private float _jobSearchCooldownInSec;

        // TODO: Should be a number
        public string ID { get; set; }

        public Vector2 Position { get { return _position; } set { _position = value; } }

        public List<JobType> Skills { get; set; }

        public List<List<IStackable>> Inventory { get; set; }

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

        // Used for picking up inventory
        public Queue<Tile> InBetweenLocations { get; set;  }

        public float Speed { get; private set; }

        public delegate void MinionHandler(Minion minion);
        public event MinionHandler MinionChanged;
        public event MinionHandler InventoryNeeded;
        public event MinionHandler RemoveInventory;

        public Minion(Tile tile, string id, float speed = 50)
        {
            CurrentTile = _currentTile = DestinationTile = tile;
            _nextTile = null;
            _position = tile.Position;
            ID = id;
            Speed = speed;
            _jobSearchCooldownInSec = 0;
            Skills = new List<JobType>();
            Skills.Add(JobType.Building);
            Skills.Add(JobType.Inventorying);
            Inventory = new List<List<IStackable>>();
        }

        public void Update(GameTime gameTime)
        {
            UpdateMovement(gameTime);
            UpdateJob(gameTime);

            //if (MinionChanged != null)
            //    MinionChanged(this);
        }

        private void UpdateMovement(GameTime gameTime)
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
                        AbandonJob();

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
                CurrentTile = _nextTile;
        }

        private void UpdateJob(GameTime gameTime)
        {
            if (_jobSearchCooldownInSec > 0)
                _jobSearchCooldownInSec -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_job == null)
            {
                if (_jobSearchCooldownInSec > 0)
                    return;

                LookForJob();

                if (_job == null)
                {
                    _jobSearchCooldownInSec = 2;

                    //DestinationTile = CurrentTile;

                    return;
                }
            }

            // There are no more required items left and minion is at destination
            if (CurrentTile == _job.Destination)
            {
                _job.DoJob((float)gameTime.ElapsedGameTime.TotalSeconds);

                _jobSearchCooldownInSec = 0;
            }
            else if (CurrentTile == DestinationTile)
            {
                // Get the inventory
                InventoryNeeded(this);

                // If there is still needed inventory 
                if (_job.RequiredItems.Count != 0 && _job.GetNextRequiredItem(Inventory) != null)
                    DestinationTile = _job.GetNextRequiredItem(Inventory);
                else
                    DestinationTile = _job.Destination;
            }
        }

        private void LookForJob()
        {
            if (JobQueue.Peek() != null && Skills.Contains(JobQueue.Peek().JobType))
                _job = JobQueue.Dequeue();

            // No job
            if (_job == null)
                return;

            if (_job.RequiredItems.Count != 0)// && _job.GetNextRequiredItem(Inventory) != null)
                DestinationTile = _job.GetNextRequiredItem(Inventory);
            else
                DestinationTile = _job.Destination;

            _job.JobStopped += OnJobStopped;
            _job.JobCancel += OnJobCancelled;

            _aStar = new AStar(CurrentTile, DestinationTile);

            // The first tile can be ignored, because that's where the minion currently is in
            _nextTile = _aStar.Pop();

            // No path was found
            if (_aStar.Length() == 0)
                AbandonJob();
        }

        public void SetDestinationTile(Tile tile)
        {
            if (_job != null)
                AbandonJob();

            DestinationTile = tile;
        }

        private void AbandonJob()
        {
            _nextTile = DestinationTile = CurrentTile;

            if (_job != null)
                JobQueue.Enqueue(_job);

            _jobSearchCooldownInSec = 15;

            _job = null;
        }

        private void OnJobStopped(Job job)
        {
            if (job == _job)
            {
                if (_job.JobType == JobType.Inventorying && DestinationTile.Stockpile != null && Inventory.Count != 0)
                    RemoveInventory(this);

                _job.JobStopped -= OnJobStopped;
                _job.JobCancel -= OnJobCancelled;

                _job = null;
            }
        }

        private void OnJobCancelled(Job job)
        {
            job.JobStopped -= OnJobStopped;
            job.JobCancel -= OnJobCancelled;

            _job = null;

            AbandonJob();
        }

        public string GetName()
        {
            return "Minion id: " + ID;
        }
    }
}
