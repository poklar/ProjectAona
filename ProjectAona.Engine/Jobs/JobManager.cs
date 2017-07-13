using Microsoft.Xna.Framework;
using ProjectAona.Engine.Jobs;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.Items;
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

        // TODO: Remove, every job needs items (I think)
        public void CreateJob(IQueueable item, Tile destination)
        {
            JobType jobType =  JobType.Idle;

            if (item.GetType() == typeof(Wall))
            {
                jobType = JobType.Building;

                Wall wall = item as Wall;

                if (destination.Wall == null)
                    wall.BlueprintType = BlueprintType.Stockpile;
                else
                    wall.BlueprintType = BlueprintType.Deconstruct;
            }
            else
            {

            }

            Job job = new Job(destination, jobType);
            job.JobObjectPrototype = item;
            job.JobComplete += OnJobComplete;
            job.JobCancel += OnJobCancel;

            destination.Blueprint = item;
            destination.IsOccupied = true;

            _jobs.Add(job, item);

            JobQueue.Enqueue(job);
        }

        public void CreateJob(IQueueable item, Tile destination, List<List<IStackable>> items)
        {
            JobType jobType = JobType.Idle;

            if (destination.Stockpile != null)
                jobType = JobType.Inventorying;

            Job job = new Job(destination, jobType, 0);
            job.JobObjectPrototype = item;
            job.JobComplete += OnJobComplete;
            job.JobCancel += OnJobCancel;
            
            if (items.Count != 0)
                job.RequiredItems = items;

            _jobs.Add(job, item);

            JobQueue.Enqueue(job);
        }



        public void CancelJob(IQueueable item, Tile tile)
        {
            Job job = null;

            foreach (var jobs in _jobs)
            {
                if (jobs.Value == item)
                    job = jobs.Key;
            }

            if (job != null)
                job.CancelJob();
        }

        private void OnJobComplete(Job job)
        {
            job.Destination.Blueprint = null;

            if (job.JobObjectPrototype!= null && job.JobObjectPrototype.GetType() == typeof(Wall))
            {
                Wall wall = job.JobObjectPrototype as Wall;

                if (job.Destination.Wall == null)
                    TerrainManager.AddWall(wall.Type, job.Destination);
                else
                    TerrainManager.RemoveWall(job.Destination);
            }

            job.JobComplete -= OnJobComplete;
            job.JobCancel -= OnJobCancel;
            _jobs.Remove(job);
            job = null;
        }

        private void OnJobCancel(Job job)
        {
            job.Destination.Blueprint = null;
            job.JobComplete -= OnJobComplete;
            job.JobCancel -= OnJobCancel;
            _jobs.Remove(job);
            JobQueue.Remove(job);
            job = null;
        }
    }
}
