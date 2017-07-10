using Microsoft.Xna.Framework;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Pathfinding;
using ProjectAona.Engine.Tiles;
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
            _jobSearchCooldownInSec = 0;
            Skills = new List<JobType>();
            Skills.Add(JobType.Building);
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
                    _jobSearchCooldownInSec = 10;

                    //DestinationTile = CurrentTile;

                    return;
                }
            }

            if (CurrentTile == _job.Tile)
            {
                _job.DoJob((float)gameTime.ElapsedGameTime.TotalSeconds);

                _jobSearchCooldownInSec = 0;
            }
        }

        private void LookForJob()
        {
            if (JobQueue.Peek() != null && Skills.Contains(JobQueue.Peek().JobType))
                _job = JobQueue.Dequeue();

            // No job
            if (_job == null)
                return;

            DestinationTile = _job.Tile;

            _job.JobStopped += OnJobStopped;

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
            job.JobStopped -= OnJobStopped;

            _job = null;
        }

        public string GetName()
        {
            return "Minion id: " + ID;
        }
    }
}
