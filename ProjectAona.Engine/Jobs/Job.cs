using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.Items;
using ProjectAona.Engine.World.Items.Resources;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.Jobs
{
    public class Job
    {
        protected float _jobTimer;
        protected bool _jobRepeats;

        public Tile Destination { get; private set; }

        // Item + itemnumber
        public Dictionary<IStackable, int> RequiredItems { get; set; }

        public JobType JobType { get; private set; }

        public IQueueable JobObjectPrototype { get; set; } // TODO: Do I need this?

        public IQueueable JobObject { get; set; } // TODO: Do I need this? // Finished product

        public float JobTimer { get { return _jobTimer; } set { _jobTimer = value;  } } // In seconds

        public delegate void JobHandler(Job job);
        public event JobHandler JobComplete;
        public event JobHandler JobCancel;
        public event JobHandler JobStopped;
        
        public Job(Tile tile, JobType jobType, float jobTimer = 5f)
        {
            Destination = tile;
            JobType = jobType;
            _jobTimer = jobTimer;
            _jobRepeats = false;
            RequiredItems = new Dictionary<IStackable, int>();
        }

        public void DoJob(float workTime)
        {
            JobTimer -= workTime;

            if (JobTimer <= 0)
            {
                if (JobComplete != null)
                    JobComplete(this);

                if (!_jobRepeats)
                    if (JobStopped != null)
                        JobStopped(this);
            }
        }

        public void CancelJob()
        {
            JobCancel(this);
        }

        public Tile GetNextRequiredItem(Dictionary<IStackable, int> items)
        {
            if (RequiredItems.Count == 0)
                return null;

            foreach (var reqiredItem in RequiredItems)
            {
                if (items.Count != 0)
                {
                    foreach (var minionItem in items)
                    {

                        if (reqiredItem.Key.GetType() == typeof(Material) &&
                            minionItem.Key.GetType() == typeof(Material) &&
                            reqiredItem.Key.ItemName == minionItem.Key.ItemName &&
                            reqiredItem.Value > minionItem.Value)
                        {
                            Material material = reqiredItem.Key as Material;
                            return material.Tile;
                        }
                        else if (reqiredItem.Key.GetType() == typeof(Food) &&
                                    minionItem.Key.GetType() == typeof(Food) &&
                                    reqiredItem.Value > minionItem.Value)
                        {
                            Food food = reqiredItem.Key as Food;
                            return food.Tile;
                        }
                        // TODO: Add furniture 
                        else if (reqiredItem.Value != minionItem.Value)
                        {
                            if (reqiredItem.Key.GetType() == typeof(Material))
                            {
                                Material material = reqiredItem.Key as Material;
                                return material.Tile;
                            }
                            else if (reqiredItem.Key.GetType() == typeof(Food))
                            {
                                Food food = reqiredItem.Key as Food;
                                return food.Tile;
                            }
                        }
                    }
                }
                else
                {
                    if (reqiredItem.Key.GetType() == typeof(Material))
                    {
                        Material material = reqiredItem.Key as Material;
                        return material.Tile;
                    }
                    else if (reqiredItem.Key.GetType() == typeof(Food))
                    {
                        Food food = reqiredItem.Key as Food;
                        return food.Tile;
                    }
                }
            }
            

            return null;
        }
    }
}
