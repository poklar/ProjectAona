using Microsoft.Xna.Framework;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.TerrainObjects;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.Jobs
{
    public class JobManager
    {
        private Dictionary<Job, IQueueable> _jobs;
        
        private TerrainManager _terrainManager;

        public JobManager(TerrainManager terrainManager)
        {
            _terrainManager = terrainManager;
            _jobs = new Dictionary<Job, IQueueable>();
        }

        public void Update(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void CreateJob(IQueueable item, Tile tile)
        {
            JobType jobType =  JobType.Idle;

            if (item.GetType() == typeof(Wall))
            {
                jobType = JobType.Building;

                Wall wall = item as Wall;
                wall.BlueprintType = BlueprintType.Stockpile;
            }

            Job job = new Job(tile, jobType);
            job.JobObjectPrototype = item;
            job.JobComplete += OnJobComplete;
            job.JobCancel += OnJobCancel;

            tile.Enterability = EnterabilityType.IsEnterable;
            tile.Blueprint = item;
            tile.IsOccupied = true;

            _jobs.Add(job, item);

            JobQueue.Enqueue(job);
        }

        private void OnJobComplete(Job job)
        {
            if (job.JobObjectPrototype.GetType() == typeof(Wall))
            {
                Wall wall = job.JobObjectPrototype as Wall;

                TerrainManager.AddWall(wall.Type, job.Tile);
            }

            job.JobComplete -= OnJobComplete;
            job.JobCancel -= OnJobCancel;
            _jobs.Remove(job);
            job = null;
        }

        private void OnJobCancel(Job job)
        {

        }
    }
}
