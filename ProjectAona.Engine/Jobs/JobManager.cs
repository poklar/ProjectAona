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

            if (_jobs.Count != 0)
                foreach (Job job in _jobs.Keys.ToList()) //TODO: Fix! Creating a list is expensive: see https://stackoverflow.com/questions/604831/collection-was-modified-enumeration-operation-may-not-execute
                    job.JobTimer -= (float)elapsedTime;
        }

        public void CreateJob(IQueueable item, Tile tile)
        {
            JobType jobType =  JobType.Idle;

            if (item.GetType() == typeof(Wall))
                jobType = JobType.Building;

            Job job = new Job(tile, jobType);
            job.JobObjectPrototype = item;
            job.JobComplete += OnJobComplete;
            job.JobCancel += OnJobCancel;

            _jobs.Add(job, item);
        }

        private void OnJobComplete(IQueueable item, Job job)
        {
            if (item.GetType() == typeof(Wall))
            {
                Wall wall = item as Wall;

                TerrainManager.AddWall(wall.Type, job.Tile);
            }

            job.JobComplete -= OnJobComplete;
            job.JobCancel -= OnJobCancel;
            _jobs.Remove(job);
            job = null;
        }

        private void OnJobCancel(IQueueable item, Job job)
        {

        }
    }
}
